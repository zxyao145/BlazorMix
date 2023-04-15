using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BlazorMix.Docs.SourceGenerators
{
    [Generator]
    public class AppRoutesGenerator : ISourceGenerator
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

        private const string RouteAttributeName = "Microsoft.AspNetCore.Components.RouteAttribute";

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                Debugger.Launch();
                var allRoutePaths = GetRouteTemplates(context.Compilation);

                var dictSource = SourceText.From(
                    AppRoutes(allRoutePaths), 
                    Encoding.UTF8);
                context.AddSource("AppRoutes", dictSource);
            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
        }

        private static ImmutableArray<string> GetRouteTemplates(Compilation compilation)
        {
            // Get all classes
            IEnumerable<SyntaxNode> allNodes = 
                compilation.SyntaxTrees
                .SelectMany(s => s.GetRoot()
                    .DescendantNodes()
                );
            IEnumerable<ClassDeclarationSyntax> allClasses = 
                allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>();

            return allClasses
                .Select(component => GetRoutePath(compilation, component))
                .Where(route => route != null)
                .Cast<string>()// stops the nullable lies
                .ToImmutableArray();
        }

        private static string? GetRoutePath(Compilation compilation, ClassDeclarationSyntax component)
        {
            var routeAttribute = component.AttributeLists
                .SelectMany(x => x.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == RouteAttributeName);

            if (routeAttribute?.ArgumentList?.Arguments.Count != 1)
            {
                // no route path
                return null;
            }

            var semanticModel = compilation.GetSemanticModel(component.SyntaxTree);

            var routeArg = routeAttribute.ArgumentList.Arguments[0];
            var routeExpr = routeArg.Expression;
            return semanticModel.GetConstantValue(routeExpr).ToString();
        }
    
    
        private static List<string> GetComponents(SyntaxNode root)
        {

            // 选择给 Program 的附加上
            var allClasses = root
                .DescendantNodes(descendIntoTrivia: true)
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>()
                .ToList();


            foreach (ClassDeclarationSyntax classDef in allClasses)
            {
                if (classDef.BaseList != null)
                {
                    var path = classDef.SyntaxTree.FilePath;
                    if (path == null)
                    {
                        continue;
                    }
                    path = path.Replace("/", "\\");
                    if (path.Contains("RazorDeclaration\\Components"))
                    {
                        continue;
                    }
                    if (path.EndsWith(".razor.route.cs"))
                    {
                        continue;
                    }

                    var index = path!.IndexOf("RazorDeclaration", StringComparison.Ordinal);
                    var s = path.Substring(index + "RazorDeclaration".Length);
                    var relativePath = path!
                        .Split(new string[] { "RazorDeclaration" }, StringSplitOptions.RemoveEmptyEntries)[1]
                        .Substring(1);

                    var baseDir = path!.Split(new string[] { "obj\\Debug" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    var scFile = Path.Combine(baseDir, relativePath);
                    scFile = scFile.Replace(".g.cs", "");


                    var baseType = classDef.BaseList.Types.FirstOrDefault();
                    string sc = classDef.ToString();
                    if (baseType != null)
                    {
                        var baseTypeName = baseType.Type.ToString();
                        if (baseTypeName == "global::Microsoft.AspNetCore.Components.ComponentBase")
                        {
                            var componentName = classDef.Identifier.Text;
                            var sourceCode = GetComponentSource(componentName, scFile);
                            sourceProductionContext
                            .AddSource(componentName + ".razor.route.cs", sourceCode);
                        }
                    }

                }
            }
        }

    }
}
