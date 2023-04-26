using System.Data;

namespace BlazorMix;

public record BuilderItem(string Value, bool Show);

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
        var res = TransitionBuilder?.GetContentArr().ToList() ?? new List<BuilderItem>();
        res.AddRange(base.GetContentArr());

        return string.Join( " ", res.Select(x => x.Value).Distinct());
    }

    public override string Build() => ToString();

    public static implicit operator ClassBuilder(string clsStr)
    {
        var builder = new ClassBuilder();
        if (!string.IsNullOrWhiteSpace(clsStr))
        {
            builder.Add(clsStr.Trim());
        }

        return builder;
    }
}