
namespace BlazorMix;
public partial class PageIndicator
{
    public const string ClsPrefix = "bm-page-indicator";
    private const string ClsDotActive = $"{ClsPrefix}-dot-active";


    /// <summary>
    /// 总页数	
    /// </summary>
    [Parameter]
    public int Total { get; set; }

    /// <summary>
    /// 当前页（从 0 开始计数）	
    /// </summary>
    [Parameter]
    public int Current { get; set; }

    /// <summary>
    /// 方向，默认是水平方向	
    /// </summary>
    [Parameter]
    public BmDirection Direction { get; set; }

    /// <summary>
    /// 颜色（'primary' | 'white'	）
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "primary";


    protected override void OnInitialized()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Direction.GetDisplayName()}")
            .Add($"{ClsPrefix}-color-{Color}")
            .Add(() => Class?.ToString() ?? "")
            ;
        _styleBuilder.Clear()
            .Add(() => Style?.ToString() ?? "")
            ;
    }
}
