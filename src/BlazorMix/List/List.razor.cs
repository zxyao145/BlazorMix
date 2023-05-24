
namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class List : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-list";

    [Parameter]
    public ListMode Mode { get; set; } = ListMode.Default;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public StringOrRenderFragment? Header { get; set; } 


    protected override async Task OnInitializedAsync()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Mode.GetDisplayName()}")
            .Add(() => Class?.ToString() ?? "");

        await base.OnInitializedAsync();
    }
}
