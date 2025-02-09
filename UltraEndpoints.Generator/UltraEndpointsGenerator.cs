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
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource($"attributes.g.cs", SourceText.From(attributestext, Encoding.UTF8)));

            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, ct) =>
                        node is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0,
                    transform: static (ctx, ct) => (ClassDeclarationSyntax)ctx.Node
                )
                .Where(static cds => HasAttribute(cds, "UltraEndpoint"));

            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses =
                context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
            {
                Compilation compilation = source.Item1;
                foreach (var classSyntax in source.Item2)
                {
                    var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
                    if (model.GetDeclaredSymbol(classSyntax) is not INamedTypeSymbol classSymbol)
                        continue;

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
            string className = classSymbol.Name;
            string namespaceName = classSymbol.ContainingNamespace?.ToString() ?? "Generated";

            var methods = classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.GetAttributes().Any(a => a.AttributeClass?.Name.StartsWith("Ultra") == true))
                .Where(m => !m.IsImplicitlyDeclared)
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine($"namespace {namespaceName}.Generated");
            builder.AppendLine("{");

            foreach (var method in methods)
            {
                var httpAttribute = method.GetAttributes()
                    .FirstOrDefault(a => a.AttributeClass?.Name.StartsWith("Ultra") == true);
                if (httpAttribute == null) continue;

                string route = httpAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? "";
                string methodName = method.Name;
                string requestType = $"{className}{methodName}Request";
                string responseType = $"{className}{methodName}Response";

                string requestParameters = string.Join(", ",
                    method.Parameters.Select(p =>
                        $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"
                    )
                );

                INamedTypeSymbol? returnType = (method.ReturnType as INamedTypeSymbol);
                INamedTypeSymbol? actualReturnType = (returnType?.TypeArguments.FirstOrDefault() ?? returnType) as INamedTypeSymbol;

                bool inheritsResponse = actualReturnType?.BaseType?.OriginalDefinition.ToDisplayString() == "SmartHome.Common.Models.Response<T>";

                builder.AppendLine();
                builder.AppendLine($"   public record {requestType}({requestParameters});");

                if (inheritsResponse)
                {
                    responseType = actualReturnType!.ToDisplayString();
                }
                else if (actualReturnType != null)
                {
                    builder.AppendLine($"   public record {responseType}({actualReturnType.ToDisplayString()} Output) : SmartHome.Common.Models.Response<{responseType}>;");
                }

                builder.AppendLine();
                builder.AppendLine($"    public class {methodName}Endpoint : Endpoint<{requestType}, {responseType}>");
                builder.AppendLine("    {");
                builder.AppendLine($"       public required {className} _ultraEndpoint {{ get; set; }}");
                builder.AppendLine("        public override void Configure()");
                builder.AppendLine("        {");
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
                builder.AppendLine("    }");
                builder.AppendLine();
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        public const string attributestext = """
using System;

namespace UltraEndpoints.Generator.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UltraEndpointAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UltraGetAttribute : Attribute
    {
        public string Route { get; }

        public UltraGetAttribute(string route)
        {
            Route = route;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UltraPostAttribute : Attribute
    {
        public string Route { get; }

        public UltraPostAttribute(string route)
        {
            Route = route;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UltraPutAttribute : Attribute
    {
        public string Route { get; }

        public UltraPutAttribute(string route)
        {
            Route = route;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UltraDeleteAttribute : Attribute
    {
        public string Route { get; }

        public UltraDeleteAttribute(string route)
        {
            Route = route;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class UltraInjectAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UltraBaseAttribute : Attribute
    {
    }
}


""";
    }

}
