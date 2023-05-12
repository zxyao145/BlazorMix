

namespace BlazorMix;
public partial class CenterPopup
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-center-popup";

    protected override Task OnInitializedAsync()
    {
        _classBuilder
            .Add($"{PrefixCls}-body")
            .Add(() => BodyClass?.ToString() ?? "");
        return base.OnInitializedAsync();
    }
}
