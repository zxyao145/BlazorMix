
using System.Security.Claims;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public abstract class BmTransitionComponentBase : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    public TransitionOptions Transition { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if(parameters.TryGetValue<TransitionOptions>(nameof(Transition), out var t))
        {
            Transition = t;
        }
        await base.SetParametersAsync(parameters);

        _classBuilder.TransitionBuilder = Transition?.Class;
        _styleBuilder.TransitionBuilder = Transition?.Style;
    }
}
