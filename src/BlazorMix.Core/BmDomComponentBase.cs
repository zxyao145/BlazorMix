namespace BlazorMix;

public abstract class BmDomComponentBase : BmComponentBase
{
    public ClassBuilder ClassBuilder { get; } = new ClassBuilder();

    protected ClassBuilder _classBuilder = new ClassBuilder();

    public StyleBuilder StyleBuilder { get; } = new StyleBuilder();
}
