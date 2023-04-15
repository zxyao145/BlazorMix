using System;

namespace BlazorMix.Docs.Internal.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class JsCustomElementAttribute : System.Attribute
{
    public JsCustomElementAttribute()
    {
    }

    public JsCustomElementAttribute(string name)
    {
        Name = name;
    }

    public string? Name { get; }
}
