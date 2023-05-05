using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorMix.Docs.SourceGenerators
{
    [Generator]
    public class ComponentsRouteGenerator : IIncrementalGenerator
    {
        public static string AppRoutes(IEnumerable<string> allRoutes)
        {
            // hard code the namespace for now
            var sb = new StringBuilder(@"
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace BlazorMix.Docs.Shared
{
    public static class AppRoutes
    {
        public static ReadOnlyCollection<string> Routes { get; } = 
            new ReadOnlyCollection<string>(
                new List<string>
                {
");
            foreach (var route in allRoutes)
            {
                sb.AppendLine($"\"{route}\",");
            }

            sb.Append(@"
                }
            );
    }
}");
            return sb.ToString();
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var compilations = context.CompilationProvider
                    // 这里的 Select 是仿照 Linq 写的，可不是真的 Linq 哦，只是一个叫 Select 的方法
                    .Select(
                        (compilation, cancellationToken) =>
                        {
                            return compilation.SyntaxTrees
                                .SelectMany(s => s.GetRoot()
                                    .DescendantNodes()
                                );
                        }
                    );
           
            context.RegisterSourceOutput(compilations,
                (sourceProductionContext, allNodes) =>
            {
                //System.Diagnostics.Debugger.Launch();

                IEnumerable<ClassDeclarationSyntax> allClasses =
                        allNodes
                        .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>();
                var components = new HashSet<string>();
                //System.Diagnostics.Debugger.Launch();

                foreach (ClassDeclarationSyntax classDef in allClasses)
                {
                    if (classDef.BaseList != null)
                    {
                        var baseType = classDef.BaseList.Types.FirstOrDefault();
                        string sc = classDef.ToString();
                        if (baseType != null)
                        {
                            var baseTypeName = baseType.Type.ToString();
                            if (baseTypeName == "global::Microsoft.AspNetCore.Components.ComponentBase")
                            {
                                // namespace
                                var namespaceDeclarationSyntax =
                                    classDef.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>();

                                // BlazorMix.Docs.Demos.Components.Button.Demos
                                var nsName = namespaceDeclarationSyntax!.Name.ToString();
                                if (nsName.Contains("BlazorMix.Docs.Demos.Components"))
                                {
                                    var componentName = nsName
                                        .Replace("BlazorMix.Docs.Demos.Components.", "")
                                        .Split('.')[0];
                                    components.Add(componentName);
                                }
                            }
                        }

                    }
                }

                Console.WriteLine(components);
                var dictSource = SourceText.From(
                      AppRoutes(components),
                      Encoding.UTF8);
                sourceProductionContext.AddSource("AppRoutes", dictSource);
            });
        }
    }
}
