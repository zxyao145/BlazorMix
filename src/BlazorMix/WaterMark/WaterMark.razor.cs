

using BlazorMix.Core;

namespace BlazorMix;
public partial class WaterMark
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-water-mark";


    private ElementReference _root;
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _classBuilder.Add(ClsPrefix)
            .AddIf($"{ClsPrefix}-full-page", () => FullPage);

        _styleBuilder.Add($"z-index:{ZIndex}")
            .Add($"background-size:{GapX + Width}px");
    }

    private WaterMarkProps? _props;
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _props = new WaterMarkProps(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JsRuntime.WaterMarkRender(_root, _props!);
        await base.OnAfterRenderAsync(firstRender);
    }
}
