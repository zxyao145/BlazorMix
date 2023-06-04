namespace BlazorMix;

public partial class Badge
{
    public const string ClsPrefix = "bm-badge";

    private OneOf<bool, string> _originContent = "";
    private string _content = "";
    private bool _isDot = true;
    /// <summary>
    /// 徽标内容。true：显示小红点；false or string：显示为徽标。
    /// </summary>
    [Parameter]
    public OneOf<bool, string> Content
    {
        get => _originContent;
        set
        {
            _originContent = value;
            if (value.IsT0)
            {
                _content = "";
                _isDot = value.AsT0;
            }
            else
            {
                _content = value.AsT1;
                _isDot = false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 徽标背景色，等效于设置 --color CSS 变量
    /// </summary>
    [Parameter] 
    public string Color { get; set; } = "";

    /// <summary>
    /// 是否增加描边	
    /// </summary>
    [Parameter]
    public bool Bordered { get; set; }

    /// <summary>
    /// Badge wrap 自定义类名
    /// </summary>
    [Parameter]
    public ClassBuilder WrapperClassName { get; set; } = new();

    /// <summary>
    /// Badge wrap 自定义样式
    /// </summary>
    [Parameter]
    public StyleBuilder WrapperStyle { get; set; } = new ();

    private readonly ClassBuilder _wrapperClassName = new();
    protected override void OnInitialized()
    {
        _classBuilder.Add(ClsPrefix)
            .Add(() => Class?.ToString() ?? "")
            .AddIf($"{ClsPrefix}-fixed", () => ChildContent != null)
            .AddIf($"{ClsPrefix}-dot", () => _isDot)
            .AddIf($"{ClsPrefix}-bordered", () => Bordered);

        _styleBuilder
            .AddIf($"--color:{Color}", () => !string.IsNullOrWhiteSpace(Color))
            .Add(() => Style?.ToString() ?? "");

        _wrapperClassName
            .Add($"{ClsPrefix}-wrapper")
            .Add(() => WrapperClassName?.ToString() ?? "");
        base.OnInitialized();
    }
}
