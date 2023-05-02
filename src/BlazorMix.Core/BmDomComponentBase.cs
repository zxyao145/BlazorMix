namespace BlazorMix;

public abstract class BmDomComponentBase : BmComponentBase
{
    /// <summary>
    /// AniOptions is a parameter specifically designed for component Animation, please use it with caution
    /// </summary>
    [CascadingParameter]
    public AniOptions? AniOptions { get; set; } //= new();
    
    [Parameter]
    public ClassBuilder? Class { get; set; }

    [Parameter]
    public StyleBuilder? Style { get; set; }

    private readonly ClassBuilder _class = new();
    private readonly StyleBuilder _stye = new();

    protected ClassBuilder _classBuilder => _class;
    protected StyleBuilder _styleBuilder => _stye;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<AniOptions?>(nameof(AniOptions), out var t))
        {
            AniOptions = t;
        }
        await base.SetParametersAsync(parameters);
        _classBuilder.TransitionBuilder = AniOptions?.Class;
        _styleBuilder.TransitionBuilder = AniOptions?.Style;
    }

}
