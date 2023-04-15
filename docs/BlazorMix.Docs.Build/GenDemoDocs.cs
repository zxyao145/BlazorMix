using Markdig.Extensions.Yaml;
using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig.Syntax;
using Markdig.Renderers;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

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
        var componentsRootDir = Path.Combine(AppContext.BaseDirectory, _options.Src);
        var components = Directory.GetDirectories(componentsRootDir);
        foreach (var component in components)
        {
            var componentName = Path.GetFileName(component);
            var docDir = Path.Combine(component, "Docs");
            // get md file of all language of one components Docs
            var docs = Directory.GetFiles(docDir);
            foreach (var docPath in docs)
            {
                // parse one language docs
                var html = ParseDoc(docPath, componentName);
                // Button\Docs\index.zh-cn.md
                var outputRelaPath = GetRelativePath(componentsRootDir, docPath);

                var output = Path.Combine(AppContext.BaseDirectory, _options.Out, "Components/", outputRelaPath);

                output = Path.ChangeExtension(output, "html");
                Console.WriteLine(output);

                var dir = Path.GetDirectoryName(output);
                if(!Directory.Exists(dir))
                { 
                    Directory.CreateDirectory(dir);
                }

              
                File.WriteAllText(output, html.Item2);
            }
        }
    }

    private static (string, string) ParseDoc(string path, string componentName)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        var lang = fileName.Split('.')[1];

        var docsText = File.ReadAllText(path);

        var pipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .Build();

        var document = Markdown.Parse(docsText, pipeline);

        var sb = new StringBuilder();
        for (var i = 0; i < document.Count; i++)
        {
            var block = document[i];
            if(block is QuoteBlock quoteBlock)
            {
                var firstLine = quoteBlock.ToList()[0];
                if(firstLine is ParagraphBlock { Inline: { } } paragraph)
                {
                    var content = string.Join("", paragraph.Inline);
                    if(content.Equals("[xmldoc]", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var trs = ReadXmlDoc(componentName);

                        var tableSb = new StringBuilder();
                        tableSb.Append(@"<div><table class=""xmldoc-table"">
	<thead>
		<tr>
			<th>Property</th>
			<th>Type</th>
			<th>Default</th>
			<th>Description</th>
		</tr>
	</thead>
	
	<tbody>");
                        tableSb.Append(trs);
                        tableSb.Append(@"</tbody>
</table></div>");
                        sb.Append(tableSb);
                        continue;
                    }
                }
                RenderToHtml(pipeline, sb, block);
            }
            RenderToHtml(pipeline, sb, block);
        }

        return (lang, sb.ToString());
    }
    static Assembly assembly = null;
    static XDocument xmlDoc = null;
    static Dictionary<string, XElement> members = null;
    static string libName = "BlazorMix";

    private static StringBuilder ReadXmlDoc(string componentName)
    {
        var sb = new StringBuilder();
        if(assembly == null)
        {
            assembly = Assembly.Load(libName);
            var location = assembly.Location;
            var xmlDocPath = Path.ChangeExtension(location, ".xml");
            // 'F:\Git\BcdLib\BlazorMixUi\BlazorMix.xml
            xmlDoc = XDocument.Load(xmlDocPath);
            members = xmlDoc.Descendants("member")
                .Where(x => x.Attribute("name")?.Value.StartsWith("P:") ?? false)
                .ToDictionary(x => x.Attribute("name").Value, x => x);
        }
        var types = assembly.GetTypes();
        var componentCls = assembly.GetType($"{libName}.Button");
        if(componentCls == null)
        {
            return sb;
        }

        var defaultInstance = Activator.CreateInstance(componentCls);

        var allParamters = componentCls
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x =>
                x.GetCustomAttribute<ParameterAttribute>() != null
                ||
                x.GetCustomAttribute<CascadingParameterAttribute>() != null
            )
            .ToList();
        foreach (var item in allParamters)
        {
            var fullName = $"P:{libName}.{componentName}.{item.Name}";
            var xmlEle = members[fullName];
            var Description = item.Name;
            if (xmlEle != null)
            {
                var summary = xmlEle.Elements("summary").FirstOrDefault();
                if(summary != null)
                {
                    Description = summary.Value.Trim();
                }
            }
            var defaultEle = xmlEle.Elements("default").FirstOrDefault();

            sb.Append("<tr>");
            sb.Append($"<td>{item.Name}</td>");
            sb.Append($"<td>{GetTypeName(item.PropertyType)}</td>");
            sb.Append($"<td>{GetDefaultValue(item, defaultInstance, defaultEle)}</td>");
            sb.Append($"<td>{Description}</td>");
            sb.Append("</tr>");
        }

        return sb;
    }

    private static string GetDefaultValue(PropertyInfo propertyInfo, object instance, XElement defaultEle)
    {
        if(defaultEle != null)
        {
            return defaultEle.Value.Trim();
        }
        var type = propertyInfo.PropertyType;
        if (type.IsEnum)
        {
            var enumVal = (Enum)propertyInfo.GetValue(instance);
            var display = enumVal.GetAttribute<DisplayAttribute>();
            if (display != null)
            {
                return display.Name;
            }
            return enumVal.ToString();
        }
        if (type.IsPrimitive || type == typeof(string))
        {
            var val = propertyInfo.GetValue(instance).ToString();
            return val;
        }
        return "";
    }

    private static string GetTypeName(Type type)
    {
        var name = type.Name;
        if (!type.IsGenericType)
        {
            return name;
        }
        name = name.Split('`')[0];
        var argeuments = type.GetGenericArguments();
        var typeNameList = argeuments.Select(x => GetTypeName(x));
        var typeNames = string.Join(", ", typeNameList);    
        name += $"&lt;{typeNames}&gt;";
        return name;
    }

    private static void RenderToHtml(MarkdownPipeline pipeline, StringBuilder sb, Block lastBlock)
    {
        using StringWriter writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        pipeline.Setup(renderer);
        var blockHtml = renderer.Render(lastBlock);
        sb.Append(blockHtml);
    }



    /// <summary>
    /// 计算相对路径
    /// 后者相对前者的路径。
    /// </summary>
    /// <param name="mainDir">主目录</param>
    /// <param name="fullFilePath">文件的绝对路径</param>
    /// <returns>fullFilePath相对于mainDir的路径</returns>
    /// <example>
    /// @"..\..\regedit.exe" = GetRelativePath(@"D:\Windows\Web\Wallpaper\", @"D:\Windows\regedit.exe" );
    /// </example>
    public static string GetRelativePath(string mainDir, string fullFilePath)
    {
        if (!mainDir.EndsWith("\\"))
        {
            mainDir += "\\";
        }

        int intIndex = -1, intPos = mainDir.IndexOf('\\');

        while (intPos >= 0)
        {
            intPos++;
            if (string.Compare(mainDir, 0, fullFilePath, 0, intPos, true) != 0) break;
            intIndex = intPos;
            intPos = mainDir.IndexOf('\\', intPos);
        }

        if (intIndex >= 0)
        {
            fullFilePath = fullFilePath.Substring(intIndex);
            intPos = mainDir.IndexOf("\\", intIndex);
            while (intPos >= 0)
            {
                fullFilePath = "..\\" + fullFilePath;
                intPos = mainDir.IndexOf("\\", intPos + 1);
            }
        }

        return fullFilePath;
    }
}
