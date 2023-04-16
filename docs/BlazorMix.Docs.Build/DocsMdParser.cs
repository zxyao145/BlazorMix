using Markdig.Renderers;
using Markdig.Syntax;
using Markdig;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Markdig.Extensions.Tables;

namespace BlazorMix.Docs.Build;
internal class DocsMdParser
{
    /// <summary>
    /// 解析渲染一个 md  文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="componentName"></param>
    /// <returns></returns>
    public static (string, string) ParseMdFile(string path, string componentName)
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
            if (block is QuoteBlock quoteBlock)
            {
                if (TryRenderXmlDoc(quoteBlock, componentName, out var tableSb))
                {
                    sb.Append(tableSb);
                    continue;
                }
                RenderToHtml(pipeline, sb, block);
            }
            else if (block is Table tableBlock)
            {
                var writer = GetHtml(pipeline, tableBlock);

                sb.Append($"<div>{writer.ToString()}</div>");
                continue;
            }
            RenderToHtml(pipeline, sb, block);
        }

        return (lang, sb.ToString());
    }

    #region xmldoc 解析

    private static bool TryRenderXmlDoc(
        QuoteBlock quoteBlock,
        string componentName,
        out StringBuilder tableSb
        )
    {
        var firstLine = quoteBlock.ToList()[0];
        if (firstLine is ParagraphBlock { Inline: { } } paragraph)
        {
            var content = string.Join("", paragraph.Inline);
            if (content.Equals("[xmldoc]", StringComparison.CurrentCultureIgnoreCase))
            {
                var trs = ReadXmlDoc(componentName);

                tableSb = new StringBuilder();
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
                tableSb.Append(@"</tbody></table></div>");
                return true;
            }
        }
        tableSb = null;
        return false;
    }

    static Assembly assembly = null;
    static XDocument xmlDoc = null;
    static Dictionary<string, XElement> members = null;
    static string libName = "BlazorMix";

    private static StringBuilder ReadXmlDoc(string componentName)
    {
        var sb = new StringBuilder();
        if (assembly == null)
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
        var componentCls = assembly.GetType($"{libName}.{componentName}");
        if (componentCls == null)
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
            var Description = item.Name;
            XElement defaultEle = null;
            if (members.ContainsKey(fullName))
            {
                var xmlEle = members[fullName];
                if (xmlEle != null)
                {
                    var summary = xmlEle.Elements("summary").FirstOrDefault();
                    Description = GetSummaryText(summary);
                }
                defaultEle = xmlEle.Elements("default").FirstOrDefault();
            }


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
        if (defaultEle != null)
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

    private static string GetSummaryText(XElement element)
    {
        if (element == null)
        {
            return "";
        }
        if (!element.HasElements)
        {
            return element.Value.Trim();
        }
        var description = "";

        var nodes = element.Nodes().ToList();
        foreach (var node in nodes)
        {
            if (node.NodeType == XmlNodeType.Text)
            {
                var ele = (XText)node;
                description += ele.Value.Trim();
            }
            else if (node.NodeType == XmlNodeType.Element)
            {
                var ele = (XElement)node;
                if (ele.Name == "see")
                {
                    var cref = ele.Attribute("cref");
                    if (cref != null)
                    {
                        var crefVal = cref.Value.Trim();
                        if (!string.IsNullOrWhiteSpace(crefVal))
                        {
                            description += $"<code>{crefVal.Split("BlazorMix.")[1]}</code>";
                        }
                    }
                }
                if (ele.Name == "c")
                {
                    description += $"<code>{ele.Value.Trim()}</code>";
                }
                if (ele.Name == "code")
                {
                    //  <pre class="language-c#"><code>@sourceCode</code></pre>
                    description += $"<code>{ele.Value.Trim()}</code>";
                }
                else
                {
                    description += GetSummaryText(ele);
                }
            }
            description += " ";
        }
        return description;
    }

    #endregion

    /// <summary>
    /// 将md渲染为html
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="sb"></param>
    /// <param name="lastBlock"></param>
    private static void RenderToHtml(MarkdownPipeline pipeline, StringBuilder sb, Block lastBlock)
    {
        var blockHtml = GetHtml(pipeline, lastBlock);
        sb.Append(blockHtml);
    }

    /// <summary>
    /// 将md渲染为html
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="sb"></param>
    /// <param name="lastBlock"></param>
    private static object GetHtml(MarkdownPipeline pipeline, Block lastBlock)
    {
        using StringWriter writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        pipeline.Setup(renderer);
        var blockHtml = renderer.Render(lastBlock);
        return blockHtml;
    }
}
