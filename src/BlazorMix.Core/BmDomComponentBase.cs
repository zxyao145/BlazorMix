namespace BlazorMix;

public abstract class BmDomComponentBase : BmComponentBase
{
    [Parameter]
    public ClassBuilder Class { get; set; } = new ClassBuilder();

    [Parameter]
    public StyleBuilder Style { get; set; } = new StyleBuilder();

    private ClassBuilder _class = new ClassBuilder();
    private StyleBuilder _stye = new StyleBuilder();

    protected ClassBuilder _classBuilder => _class;
    protected StyleBuilder _styleBuilder => _stye;
}
