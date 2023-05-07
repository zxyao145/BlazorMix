using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace BlazorMix.Docs.Build;

#nullable enable 
public class XmlDocHelper
{
    private readonly string _libName = "BlazorMix";


    public XmlDocHelper(string? libName = null)
    {
        _libName = libName ?? "BlazorMix";
        _assembly = Assembly.Load(_libName);
        if (_assembly == null)
        {
            throw new ArgumentException($"{_libName} dll not found!");
        }
        // var location = assembly.Location;
        var dir = Path.Combine(AppContext.BaseDirectory, "xmldocs");
        _xmlDefaultDocPath = Path.Combine(dir, _libName + ".xml");
        // 'F:\x\x\BlazorMixUi\BlazorMix.xml
        _defaultXmlDoc = XDocument.Load(_xmlDefaultDocPath);
    }

    private readonly Assembly _assembly;
    // 从 dll 直接生成 的 xmldoc 的路径
    private readonly string _xmlDefaultDocPath ;
    // 从 dll 直接生成 的 xmldoc
    private readonly XDocument _defaultXmlDoc;


    private readonly Dictionary<string, Dictionary<string, XElement>> AllLangXmlDoc = new();
    private readonly Dictionary<Type, object?> Instances
        = new ();

    private readonly Dictionary<string, string> DefaultValuesCache = new();

    private readonly Dictionary<string, string> DefaultTypeNameCacheCache = new();

    private readonly List<string> IgnoreParameterName = new List<string>()
    {
        "AniOptions",
        "ChildContent",
        "Class",
        "Style"
    };

    private void MakeSureLanguageXmlDoc(string language)
    {
        if (!AllLangXmlDoc.ContainsKey(language))
        {
            var languageXmlDocPath = Path.ChangeExtension(_xmlDefaultDocPath, $".{language}.xml");

            var tempMembers =
                _defaultXmlDoc
                    .Descendants("member")
                    .ToDictionary(x => x.Attribute("name")!.Value, x => x);

            //_defaultXmlDoc
            //    .Descendants("member")
            //    .Where(x => x.Attribute("name")?.Value.StartsWith("P:") ?? false)
            //    .ToDictionary(x => x.Attribute("name").Value, x => x);

            if (File.Exists(languageXmlDocPath))
            {
                var xmlDoc = XDocument.Load(languageXmlDocPath);
                var languageMembers =
                    xmlDoc
                        .Descendants("member")
                    .ToDictionary(x => x.Attribute("name")!.Value, x => x);
                foreach (var member in languageMembers)
                {
                    tempMembers[member.Key] = member.Value;
                }
            }

            AllLangXmlDoc.Add(language, tempMembers);
        }
    }

    public StringBuilder ReadComponentXmlDocToMdTable(string className, string language)
    {
        Func<PropertyInfo, bool> propertyFilter = x => x.GetCustomAttribute<ParameterAttribute>() != null
                                                       ||
                                                       x.GetCustomAttribute<CascadingParameterAttribute>() != null;
        return ReadClassPropertyMdTable(className, language, propertyFilter) ;
    }

    public StringBuilder ReadClassPropertyMdTable(
        string className,
        string language,
        Func<PropertyInfo, bool>? propertyFilter = null)
    {
        Console.WriteLine("className: {0}", className);
        if (className == "Mask")
        {
            Console.WriteLine("debug");
        }
        var sb = new StringBuilder();

        MakeSureLanguageXmlDoc(language);
        var classType = _assembly.GetType($"{_libName}.{className}");
        if (classType == null)
        {
            return sb;
        }
        if (!Instances.ContainsKey(classType))
        {
            Instances[classType] = Activator.CreateInstance(classType)!;
        }
        if (Instances[classType] == null)
        {
            return sb;
        }
        var defaultInstance = Instances[classType]!;



        var members = AllLangXmlDoc[language];

        var q = classType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .AsEnumerable();
        if (propertyFilter != null)
        {
            q = q.Where(propertyFilter);
        }

        var allParameters = q.ToList();

        foreach (var propertyInfo in allParameters)
        {
            if (IgnoreParameterName.Contains(propertyInfo.Name))
            {
                continue;
            }
            // propertyInfo 是复杂类型
            var componentName = className;
            var instance = defaultInstance!;
            if (propertyInfo.DeclaringType != null && propertyInfo.DeclaringType != classType)
            {
                componentName = propertyInfo.DeclaringType.Name;
                if (!propertyInfo.DeclaringType.IsAbstract)
                {
                    if (!Instances.ContainsKey(propertyInfo.DeclaringType))
                    {
                        Instances[propertyInfo.DeclaringType] = Activator.CreateInstance(propertyInfo.DeclaringType);
                    }

                    instance = Instances[classType];
                }
            }

            var fullName = $"P:{_libName}.{componentName}.{propertyInfo.Name}";
            var description = propertyInfo.Name;
            XElement? defaultEle = null;
            if (members.TryGetValue(fullName, out XElement? xmlEle))
            {
                if (xmlEle == null)
                {
                    defaultEle = xmlEle?.Elements("default").FirstOrDefault();
                }
                else
                {
                    var summary = xmlEle.Elements("summary").FirstOrDefault();
                    description = GetSummaryText(summary);
                }
            }

            // todo optimize
            sb.Append("<tr>");
            sb.Append($"<td>{propertyInfo.Name}</td>");
            sb.Append($"<td>{GetTypeName(propertyInfo.PropertyType)}</td>");
            sb.Append($"<td>{GetDefaultValue(componentName, propertyInfo, instance, defaultEle)}</td>");
            sb.Append($"<td>{description}</td>");
            sb.Append("</tr>");
        }

        return sb;
    }
    
    private string GetDefaultValue(
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
                try
                {
                    var val = propertyInfo.GetValue(instance).ToString();
                    return val;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return "";
        }

        string key = componentName + propertyInfo.Name;
        if (!DefaultValuesCache.ContainsKey(key))
        {
            var val = DoGetDefaultVal();
            DefaultValuesCache[key] = val;
        }

        return DefaultValuesCache[key];
    }


    private string GetTypeName(Type type)
    {
        string key = type.FullName;
        if (!DefaultTypeNameCacheCache.ContainsKey(key))
        {
            DefaultTypeNameCacheCache[key] = DoGetTypeName(type);
        }
        return DefaultTypeNameCacheCache[key];
    }

    private string DoGetTypeName(Type type)
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
}
