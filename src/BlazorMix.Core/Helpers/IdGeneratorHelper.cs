namespace BlazorMix;

public static class IdGeneratorHelper
{
    public static string Generate()
    {
        return Generate("bm-");
    }

    public static string Generate(string prefix)
    {
        return prefix + Guid.NewGuid();
    }
}
