using BlazorMix.Helpers;
using System.Data;

namespace BlazorMix;
public class ClassBuilder : BuilderBase
{
    public ClassBuilder() : base()
    {

    }
    public ClassBuilder(ClassBuilder builder) : base(builder)
    {

    }

    public override string ToString()
    {
        return string.Join(
        " ",
            Items.Where(x => x.Value())
                .Select(x => x.Key())
            );
    }

    public override string Build() => ToString();

    public static implicit operator ClassBuilder(string clsStr)
    {
        var builder = new ClassBuilder();
        if(!string.IsNullOrWhiteSpace(clsStr))
        {
            builder.Add(clsStr.Trim());
        }

        return builder;
    }
}