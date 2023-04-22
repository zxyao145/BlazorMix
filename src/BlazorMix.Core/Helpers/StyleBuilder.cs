using System.Data;

namespace BlazorMix;
public class StyleBuilder : BuilderBase
{
    public StyleBuilder() : base()
    {

    }
    public StyleBuilder(StyleBuilder builder) : base(builder)
    {

    }

    public override string ToString()
    {
        return (TransitionBuilder?.ToString() ?? "") + ";" + string.Join(
        ";",
            Items.Where(x => x.Value())
                .Select(x => x.Key())
            );
    }

    public override string Build() => ToString();

    public static implicit operator StyleBuilder(string style)
    {
        var builder = new StyleBuilder();
        if (!string.IsNullOrWhiteSpace(style))
        {
            builder.Add(style.Trim());
        }

        return builder;
    }
}
