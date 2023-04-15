using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Net.WebRequestMethods;

namespace BlazorMix.Docs.SourceGenerators
{
    [Generator]
    public class PageGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            System.Diagnostics.Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Get all classes
            var allNodes = context.Compilation
                .SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes())
                .ToList();
            var allClasses = allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>().ToList();

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
                            context.AddSource(componentName + ".razor.route.cs", sourceCode);
                        }
                    }

                }
            }

            Console.WriteLine("aa");
        }

        private static string GetComponentSource(string componentName, string scFile)
        {
            var lines = System.IO.File.ReadAllLines(scFile);
            var newLines = new List<string>(lines.Length + 1);
            bool findRoute = false;
            bool findNamespace = false;

            foreach ( var item in lines)
            {
                var line = item;
                if (!findRoute)
                {
                    if (
                        line.Contains("public partial class")
                        && line.Contains("global::Microsoft.AspNetCore.Components.ComponentBase")
                        )
                    {
                        var route = "[global::Microsoft.AspNetCore.Components.RouteAttribute(" + $"/components/{componentName.ToLower()}" + ")]";
                        newLines.Add(route);
                    }
                    findRoute = true;
                }

                if (!findNamespace)
                {
                    if (
                       line.Contains("namespace BlazorMix.Docs.Components")
                       )
                    {
                        line = line.Replace("namespace BlazorMix.Docs.Components", "namespace BlazorMix.Docs.Components.Pages");
                    }

                    findNamespace = true;
                }
                
                newLines.Add(line);
            }

            return string.Join(Environment.NewLine, newLines);
        }
        private static string GetComponentSource(string componentName)
        {
            string sourceCode = @"

namespace BlazorMix.Docs.Components.PageGenerator;

[global::Microsoft.AspNetCore.Components.RouteAttribute(""/components/" + componentName.ToLower() + @""")]
public partial class " + componentName + @" : global::Microsoft.AspNetCore.Components.ComponentBase
{
};";
            return sourceCode;
        }
    }
}
