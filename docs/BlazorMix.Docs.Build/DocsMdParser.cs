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
using GTranslate;
using System.Collections;
using Markdig.Syntax.Inlines;

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
                continue;
            }
            else if (block is Table tableBlock)
            {
                var writer = GetHtml(pipeline, tableBlock);

                sb.Append($"<div>{writer.ToString()}</div>");
                continue;
            }
            else if (block is CodeBlock codeBlock)
            {
                var writer = GetHtml(pipeline, codeBlock);
                var xml = XDocument.Parse(writer.ToString()!);
                var code = xml.Descendants("code").ToList()[0];
                var l = code.Attribute("class")?.Value ?? "";
                xml.Root.SetAttributeValue("class", l + " page-code");
                var s = xml.ToString(SaveOptions.DisableFormatting);
                sb.Append($"{s}");
                continue;
            }
            RenderToHtml(pipeline, sb, block);
        }

        return (lang, sb.ToString());
    }

    #region xmldoc 解析

    static XmlDocHelper xmlDocHelper = new XmlDocHelper();

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
            StringBuilder trs = null;

            if (paragraph.Inline.FirstChild is LinkInline url)
            {
                var label = ((LiteralInline)url.FirstChild).Content.ToString();
                if (label == "xmldoc")
                {
                    var classInfo = url.Url!.Trim().Split("#");
                    Func<PropertyInfo, bool> propertyFilter = null;
                    if (classInfo.Length > 1)
                    {
                        propertyFilter = p => p.Name == classInfo[1];
                    }

                    trs = xmlDocHelper.ReadClassPropertyMdTable(classInfo[0], language, propertyFilter);
                }
            }
            else
            {
                var content = string.Join("", paragraph.Inline).Trim();
                if (content.StartsWith("[xmldoc]", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (content == "[xmldoc]")
                    {
                        trs = xmlDocHelper.ReadComponentXmlDocToMdTable(componentName, language);
                    }
                }
            }

            if (trs != null)
            {
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
