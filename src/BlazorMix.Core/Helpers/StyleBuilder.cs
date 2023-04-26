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
        var res = TransitionBuilder?.GetContentArr().ToList() ?? new List<BuilderItem>();
        res.AddRange(base.GetContentArr());
        return string.Join("; ", res.Select(x =>
                new KeyValuePair<string, string>(x.Value.Split(":")[0], x.Value)
                )
            .DistinctBy(x => x.Key)
            .Select(x => x.Value)
            .Distinct()
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
