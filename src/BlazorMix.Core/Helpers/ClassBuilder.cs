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
}