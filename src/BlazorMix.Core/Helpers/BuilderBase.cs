using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public abstract class BuilderBase
{
    protected readonly Dictionary<Func<string>, Func<string, bool>> Items 
        = new();

    public BuilderBase? TransitionBuilder { get; set; }

    protected static Func<string, bool> AlwaysIsTrue = (str) => true;

    protected BuilderBase()
    {

    }

    protected BuilderBase(BuilderBase builderBase)
    {
        this.Items = builderBase.Items;
    }

    public BuilderBase Add(string style)
    {
        return Add(() => style);
    }

    public BuilderBase Add(BuilderBase b)
    {
        return Add(()=> b.ToString() ?? "");
    }

    public BuilderBase Add(Func<string> styleFunc)
    {
        return AddIf(styleFunc, AlwaysIsTrue);
    }

    public BuilderBase AddIf(string style, Func<bool> func)
    {
        return AddIf(() => style, str => func());
    }

    public BuilderBase AddIf(string style, Func<string, bool> func)
    {
        return AddIf(() => style, func);
    }

    public BuilderBase AddIf(Func<string> styleFunc, Func<string, bool> func)
    {
        Items[styleFunc] = func;
        return this;
    }

    public BuilderBase Clear()
    {
        Items.Clear();
        return this;
    }

    public abstract string Build();

    internal IEnumerable<BuilderItem> GetContentArr()
    {
        return Items
            .Select(x =>
            {
                var val = x.Key();
                var show = x.Value(val);
                return new BuilderItem(val, show);
            })
            .Where(x => x.Show && !string.IsNullOrWhiteSpace(x.Value));
    }

    //public static explicit operator string(BuilderBase builder)
    //{
    //    return builder?.ToString() ?? "";
    //}
}
