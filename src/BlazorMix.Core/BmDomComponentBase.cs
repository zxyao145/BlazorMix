namespace BlazorMix;

public abstract class BmDomComponentBase : BmComponentBase
{
    [Parameter]
    public ClassBuilder Class { get; set; } = new ClassBuilder();

    protected ClassBuilder _classBuilder = new ClassBuilder();

    [Parameter]
    public StyleBuilder Style { get; set; } = new StyleBuilder();
}
