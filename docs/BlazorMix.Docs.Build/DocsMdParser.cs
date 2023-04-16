﻿using Markdig.Renderers;
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
using GTranslate;
using System.Collections;

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
                if (TryRenderXmlDoc(quoteBlock, componentName, lang, out var tableSb))
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
        string language,
        out StringBuilder tableSb
        )
    {
        var firstLine = quoteBlock.ToList()[0];
        if (firstLine is ParagraphBlock { Inline: { } } paragraph)
        {
            var content = string.Join("", paragraph.Inline);
            if (content.Equals("[xmldoc]", StringComparison.CurrentCultureIgnoreCase))
            {
                var trs = ReadXmlDoc(componentName, language);

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
    static XDocument defaultXmlDoc = null;
    static string xmlDefaultDocPath = "";
    static Dictionary<string, Dictionary<string, XElement>> allLangXmlDoc 
        = new Dictionary<string, Dictionary<string, XElement>>();
    static Dictionary<Type, object> instances
        = new Dictionary<Type, object>();

    static Dictionary<string, string> defaultValusCache
      = new Dictionary<string, string>();

    static Dictionary<string, string> defaultTypeNameCacheCache
        = new Dictionary<string, string>();

    static string libName = "BlazorMix";

    private static StringBuilder ReadXmlDoc(string componentName, string language)
    {
        var sb = new StringBuilder();
        if (assembly == null)
        {
            assembly = Assembly.Load(libName);

            // var location = assembly.Location;
            var dir = Path.Combine(AppContext.BaseDirectory, "xmldocs");
            if (defaultXmlDoc == null)
            {
                xmlDefaultDocPath = Path.Combine(dir, libName + ".xml");
                // 'F:\Git\BcdLib\BlazorMixUi\BlazorMix.xml
                defaultXmlDoc = XDocument.Load(xmlDefaultDocPath);
            }
        }
        if (!allLangXmlDoc.ContainsKey(language))
        {
            var languageXml = Path.ChangeExtension(xmlDefaultDocPath, $".{language}.xml");
            var tempMembers = defaultXmlDoc.Descendants("member")
                   .Where(x => x.Attribute("name")?.Value.StartsWith("P:") ?? false)
                   .ToDictionary(x => x.Attribute("name").Value, x => x);

            if (File.Exists(languageXml))
            {
               
                var xmlDoc = XDocument.Load(languageXml);
                var langUageMembers = xmlDoc.Descendants("member")
                    .Where(x => x.Attribute("name")?.Value.StartsWith("P:") ?? false)
                    .ToDictionary(x => x.Attribute("name").Value, x => x);
                foreach (var member in langUageMembers)
                {
                    tempMembers[member.Key] = member.Value;
                }

            }

            allLangXmlDoc.Add(language, tempMembers);
        }


        var types = assembly.GetTypes();
        var componentCls = assembly.GetType($"{libName}.{componentName}");
        if (componentCls == null)
        {
            return sb;
        }

        if(!instances.ContainsKey(componentCls))
        {
            instances[componentCls] = Activator.CreateInstance(componentCls);
        }

        var defaultInstance = instances[componentCls];
        var members = allLangXmlDoc[language];


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

            // todo optimize
            sb.Append("<tr>");
            sb.Append($"<td>{item.Name}</td>");
            sb.Append($"<td>{GetTypeName(item.PropertyType)}</td>");
            sb.Append($"<td>{GetDefaultValue(componentName, item, defaultInstance, defaultEle)}</td>");
            sb.Append($"<td>{Description}</td>");
            sb.Append("</tr>");
        }

        return sb;
    }

    private static string GetDefaultValue(
        string componentName,
        PropertyInfo propertyInfo, 
        object instance, 
        XElement defaultEle
        )
    {

        string DoGetDefaultVal()
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

        string key = componentName + propertyInfo.Name;
        if(!defaultValusCache.ContainsKey(key))
        {
            var val = DoGetDefaultVal();
            defaultValusCache[key] = val;
        }

        return defaultValusCache[key];
    }


    private static string GetTypeName(Type type)
    {
        string key = type.FullName;
        if (!defaultTypeNameCacheCache.ContainsKey(key))
        {
            defaultTypeNameCacheCache[key] = DoGetTypeName(type);
        }
        return defaultTypeNameCacheCache[key];
    }


    private static string DoGetTypeName(Type type)
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
