using System;
using System.Collections.Generic;
using System.IO;

namespace BlazorMix.Docs.Build;


internal class GenDemoDocs
{
    private readonly Options _options;
    public GenDemoDocs(Options options)
    {
        _options = options;
    }

    public void Run()
    {
        string componentsRootDir;
        if (_options.Src.StartsWith("./") || _options.Src.StartsWith(".."))
        {
            componentsRootDir = Path.Combine(AppContext.BaseDirectory, _options.Src);
        }
        else
        {
            componentsRootDir = _options.Src;
        }

        var components = Directory.GetDirectories(componentsRootDir);
        var cache = new Dictionary<string, bool>();
        foreach (var component in components)
        {
            var componentName = Path.GetFileName(component);
            if (cache.ContainsKey(componentName))
            {
                continue;
            }

            cache[componentName] = true;

            ParseDocsMdFile(componentsRootDir, component, componentName);
            if (_options.Type == "full")
            {
                ParseDemoTxt(componentsRootDir, component);
            }
        }
    }

    private void ParseDocsMdFile(string componentsRootDir, string component, string componentName)
    {
        var docDir = Path.Combine(component, "Docs");
        // get md file of all language of one components Docs
        var docs = Directory.GetFiles(docDir);

        foreach (var docPath in docs)
        {
            // Button\Docs\index.zh-CN.md
            var html = DocsMdParser.ParseMdFile(docPath, componentName);

            GenerateFiles(
                componentsRootDir,
                docPath,
                _options.Out,
                html.Item2,
                ".html"
                );
        }
    }

    private void ParseDemoTxt(string componentsRootDir, string component)
    {
        var demosDir = Path.Combine(component, "Demos");
        // get md file of all language of one components Docs
        var demos = Directory.GetFiles(demosDir, "*.razor");
        foreach (var razorPath in demos)
        {
            // Button\Demos\Demo1.razor
            GenerateFiles(
                componentsRootDir,
                razorPath,
                _options.Out,
                File.ReadAllText(razorPath),
                ".txt"
                );
        }
    }

    static void GenerateFiles(
        string componentsRootDir, 
        string srcFilePath, 
        string outDir,
        string srcContent,
        string outputExtension
        )
    {
        var outputRelaPath = PathUtil.GetRelativePath(componentsRootDir, srcFilePath);
        string output;
        if (outDir.StartsWith("./") || outDir.StartsWith("../"))
        {
            output = Path.Combine(AppContext.BaseDirectory, outDir, "Components/", outputRelaPath);
        }
        else
        {
            output = Path.Combine(outDir, "Components/", outputRelaPath);
        }
        output = Path.ChangeExtension(output, outputExtension);
        Console.WriteLine($"output: {outDir}, {outputRelaPath}, {output}");
        var dir = Path.GetDirectoryName(output);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(output, srcContent);
    }

}
