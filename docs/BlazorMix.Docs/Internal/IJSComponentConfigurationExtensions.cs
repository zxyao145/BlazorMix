using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using System.Reflection;
using BlazorMix.Docs.Internal.Attribute;

namespace BlazorMix.Docs.Internal;

public static class IJSComponentConfigurationExt
{
    public static void RegisterCustomElements(this IJSComponentConfiguration componentConfiguration)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
            .Where(u => u.FullName!.Contains("BlazorMix.Docs")))
        {
            var jsCustomElementTypes = assembly
                .GetTypes()
                .Where(type => typeof(ComponentBase).IsAssignableFrom(type)
                        && type.CustomAttributes.Any(
                            attr => attr.AttributeType == typeof(JsCustomElementAttribute)
                      )
                  );
            foreach (var type in jsCustomElementTypes)
            {
                var attr = (JsCustomElementAttribute)type
                    .GetCustomAttributes(typeof(JsCustomElementAttribute)).First();
                componentConfiguration.RegisterCustomElements(attr, type);
            }
        }
    }

    private static void RegisterCustomElements(this IJSComponentConfiguration componentConfiguration,
        JsCustomElementAttribute attr,
        Type type)
    {
        var name = attr.Name;

        if (string.IsNullOrWhiteSpace(name))
        {
            name = ToKebab(type.Name);
        }

        componentConfiguration.RegisterForJavaScript
            (
                type,
                name,
                "registerBlazorCustomElement"
            );
    }


    private static Regex regex = new Regex("(?<!^)(?=[A-Z])");

    private static string ToKebab(string name)
    {
        var split = regex.Split(name).Select(s => s.Trim('-'));
        return string.Join("-", split).ToLowerInvariant();
    }

}
