
using BlazorMix.Core;

namespace BlazorMix;
public partial class CenterPopup
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-center-popup";


    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        _classBuilder
            .Add($"{PrefixCls}-body")
            .Add(() => Class?.ToString() ?? "");
    }
}
