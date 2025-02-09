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

            // Use a StringBuilder to build the generated code.
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {namespaceName}.Generated");
            builder.AppendLine("{");


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
                
                //how get full type??
                //string requestParameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type}???? {p.Name}"));
                string requestParameters = string.Join(", ",
                    method.Parameters.Select(p => 
                        $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"
                    )
                );

                // Get return type (unwrap Task<T>)
                INamedTypeSymbol returnType = (method.ReturnType as INamedTypeSymbol);
                INamedTypeSymbol? actualReturnType = (returnType?.TypeArguments.FirstOrDefault() ?? returnType) as INamedTypeSymbol;

                // Check if the return type inherits Response<T>
                bool inheritsResponse = actualReturnType?.BaseType?.ConstructedFrom?.ToDisplayString() == "SmartHome.Common.Models.Response<T>";

                builder.AppendLine();
                //generate request and respionse
                builder.AppendLine($"   public record {requestType}({requestParameters});");
                if (inheritsResponse)
                {   //it already inherits the response type so use that instead
                    responseType = actualReturnType.ToDisplayString();
                }
                else
                { 
                    builder.AppendLine($"   public record {responseType}({actualReturnType} Output) : SmartHome.Common.Models.Response<{responseType}>;");
                }
                builder.AppendLine();

                builder.AppendLine($"    public class {methodName}Endpoint : Endpoint<{requestType}, {responseType}>");
                builder.AppendLine("    {");
                builder.AppendLine();
                builder.AppendLine($"       public required {className} _ultraEndpoint {{ get; set; }}");
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
                builder.AppendLine($"           var response = await _ultraEndpoint.{methodName}({mappedParameters});");
                builder.AppendLine("            await SendAsync(response);");
                builder.AppendLine("        }");
                builder.AppendLine();
                builder.AppendLine("    }"); // End of endpoint class
                builder.AppendLine();
            }

            builder.AppendLine("}"); // End of namespace
            return builder.ToString();
        }

    }
}
