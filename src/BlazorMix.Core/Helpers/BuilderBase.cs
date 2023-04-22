using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public abstract class BuilderBase
{
    protected readonly Dictionary<Func<string>, Func<bool>> Items 
        = new();

    public BuilderBase? TransitionBuilder { get; set; }

    protected static Func<bool> AlwaysIsTrue = () => true;

    protected BuilderBase()
    {

    }

    protected BuilderBase(BuilderBase builderBase)
    {
        this.Items = builderBase.Items;
    }

    public BuilderBase Add(string style)
    {
        Items.Add(() => style, AlwaysIsTrue);
        return this;
    }

    internal BuilderBase AddRange(List<string> styles)
    {
        foreach (string style in styles)
        {
            Items.Add(() => style, AlwaysIsTrue);
        }
        return this;
    }

    public BuilderBase AddIf(string style, Func<bool> func)
    {
        Items.Add(() => style, func);
        return this;
    }

    public BuilderBase Get(Func<string> funcName)
    {
        Items.Add(funcName, AlwaysIsTrue);
        return this;
    }

    public BuilderBase GetIf(Func<string> funcName, Func<bool> func)
    {
        Items.Add(funcName, func);
        return this;
    }

    public BuilderBase Clear()
    {
        Items.Clear();
        return this;
    }

    public abstract string Build();


    //public static explicit operator string(BuilderBase builder)
    //{
    //    return builder?.ToString() ?? "";
    //}
}
