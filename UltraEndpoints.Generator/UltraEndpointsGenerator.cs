using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace UltraEndpoints.Generator
{
    [Generator]
    public class UltraEndpointsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //for debugging
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            // Create a provider for all class declarations that have any attributes.
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, ct) =>
                        node is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0,
                    transform: static (ctx, ct) => (ClassDeclarationSyntax)ctx.Node
                )
                // Filter to classes that have [UltraEndpoint] attribute.
                .Where(static cds => HasAttribute(cds, "UltraEndpoint"));

            // Combine with the compilation
            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses =
                context.CompilationProvider.Combine(classDeclarations.Collect());

            // Register source output using the combined result.
            context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
            {
                Compilation compilation = source.Item1;
                foreach (var classSyntax in source.Item2)
                {
                    // Get the semantic model for this syntax tree.
                    var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
                    if (!(model.GetDeclaredSymbol(classSyntax) is INamedTypeSymbol classSymbol))
                        continue;

                    // Generate code for the class.
                    string generatedCode = GenerateCodeForClass(classSymbol);
                    spc.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
                }
            });
        }

        private static bool HasAttribute(ClassDeclarationSyntax classSyntax, string attributeName)
        {
            return classSyntax.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(attr => attr.Name.ToString().Contains(attributeName));
        }

        private static string GenerateCodeForClass(INamedTypeSymbol classSymbol)
        {
            // Get the class name and namespace.
            string className = classSymbol.Name;
            string namespaceName = classSymbol.ContainingNamespace.ToString();

            // Find methods that have any attribute whose name starts with "Ultra"
            var methods = classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.GetAttributes().Any(a => a.AttributeClass?.Name.StartsWith("Ultra") == true))
                .Where(m => !m.IsImplicitlyDeclared)
                .ToList();

            var otherMethods = classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.GetAttributes().All(a => a.AttributeClass?.Name.StartsWith("Ultra") == false))
                .Where(m => !m.IsImplicitlyDeclared)
                .ToList();

            // Use a StringBuilder to build the generated code.
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {namespaceName}.Generated");
            builder.AppendLine("{");

            // Generate the shared base class that copies over [UltraInject] properties.
            builder.AppendLine($"    public abstract class {className}EndpointBase<T_Request, T_Response> : FastEndpoints.Endpoint<T_Request, T_Response>");
            builder.AppendLine("    {");

            //var members = classSymbol.GetMembers().OfType<MethodDeclarationSyntax>().ToList();
            // Copy properties marked with [UltraInject].
            foreach (var prop in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                if (prop.GetAttributes().Any(a => a.AttributeClass?.Name == nameof(UltraInjectAttribute)))
                {
                    builder.AppendLine($"        public {prop.Type} {prop.Name} {{ get; set; }}");
                }
            }
            foreach (var method in otherMethods)
            {
                var code = GetMethodCode(method);
                builder.AppendLine(code);
            }
                    //string.Join("\n", methods.Where(m => !m.AttributeLists.Any(a => a.Attributes.Any(attr => attr.Name.ToString().StartsWith("Ultra"))))
                    //              .Select(m => m.ToFullString()))

            builder.AppendLine("    }"); // End of base class
            builder.AppendLine();

            // Generate an endpoint class for each method with an Ultra attribute.
            foreach (var method in methods)
            {
                // Get the first attribute whose name starts with "Ultra"
                var httpAttribute = method.GetAttributes()
                    .FirstOrDefault(a => a.AttributeClass?.Name.StartsWith("Ultra") == true);
                if (httpAttribute == null)
                    continue;

                // Get the route from the attribute constructor argument.
                string route = httpAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? "";
                string methodName = method.Name;
                string requestType = $"{methodName}Request";
                string responseType = $"{methodName}Response";

                builder.AppendLine($"    public class {methodName}Endpoint : {className}EndpointBase<{requestType}, {responseType}>");
                builder.AppendLine("    {");
                builder.AppendLine("        public override void Configure()");
                builder.AppendLine("        {");
                // Remove the "Ultra" prefix and "Attribute" suffix to get the HTTP verb.
                string verb = httpAttribute.AttributeClass?.Name.Replace("Ultra", "").Replace("Attribute", "");
                builder.AppendLine($"            {verb}(\"{route}\");");
                builder.AppendLine("            AllowAnonymous();");
                builder.AppendLine("        }");
                builder.AppendLine();
                builder.AppendLine($"        public override async Task HandleAsync({requestType} request, CancellationToken ct)");
                builder.AppendLine("        {");
                string mappedParameters = string.Join(", ", method.Parameters.Select(p => $"request.{p.Name}"));
                builder.AppendLine($"            var response = await base.{methodName}({mappedParameters});");
                builder.AppendLine("            await SendAsync(response);");
                builder.AppendLine("        }");
                builder.AppendLine();
                builder.AppendLine("    }"); // End of endpoint class
                builder.AppendLine();
            }

            builder.AppendLine("}"); // End of namespace
            return builder.ToString();
        }
        private static string GetMethodCode(IMethodSymbol methodSymbol)
        {
            // Get the syntax reference for the method
            var syntaxReference = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxReference == null)
                return string.Empty;

            // Get the syntax node (MethodDeclarationSyntax)
            var methodSyntax = syntaxReference.GetSyntax() as MethodDeclarationSyntax;
            if (methodSyntax == null)
                return string.Empty;

            // Convert syntax node back to string
            return methodSyntax.ToFullString();
        }

    }
}
