
using System.Security.Claims;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public abstract class BmAniableComponentBase : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    public AniOptions AniOptions { get; set; } = new ();

    /// <summary>
    /// Triggers the enter or exit states
    /// </summary>
    /// <default>
    /// false
    /// </default>
    [Parameter]
    public bool In
    {
        get => AniOptions.In;
        set => AniOptions.In = value;
    }


    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<AniOptions>(nameof(AniOptions), out var t))
        {
            AniOptions = t;
        }

        await base.SetParametersAsync(parameters);
        
        _classBuilder.TransitionBuilder = AniOptions.Class;
        _styleBuilder.TransitionBuilder = AniOptions.Style;
    }
}
