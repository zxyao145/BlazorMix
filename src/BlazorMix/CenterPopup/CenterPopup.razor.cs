

namespace BlazorMix;
public partial class CenterPopup
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-center-popup";

    protected override Task OnInitializedAsync()
    {
        _classBuilder
            .Add($"{ClsPrefix}-body")
            .Add(() => BodyClass?.ToString() ?? "");
        return base.OnInitializedAsync();
    }
}
