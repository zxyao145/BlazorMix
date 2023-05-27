
using BlazorMix.Core;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorMix;
public partial class Ellipsis
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-ellipsis";

    /// <summary>
    /// 文本内容	
    /// </summary>
    [Parameter] 
    public string Content { get; set; } = "";

    /// <summary>
    /// 省略位置	
    /// </summary>
    [Parameter]
    public EllipsisDirection Direction { get; set; }

    /// <summary>
    /// 展示几行	
    /// </summary>
    [Parameter]
    public int Rows { get; set; } = 1;

    /// <summary>
    /// 展开操作的文案	
    /// </summary>
    [Parameter]
    public string ExpandText { get; set; } = "";

    /// <summary>
    /// 收起操作的文案	
    /// </summary>
    [Parameter]
    public string CollapseText { get; set; } = "";

    /// <summary>
    /// 是否默认展开	
    /// </summary>
    [Parameter]
    public EventCallback OnContentClick { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool DefaultExpanded { get; set; }

    private EllipsisProps _props = new EllipsisProps();

    private bool _isExpanded = false;
    private EmojiEncoder _emojiEncoder = new EmojiEncoder();
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        _calcResult = null;

        _isExpanded = DefaultExpanded;

        _props.Rows = Rows;
        _props.Direction = Direction.GetDisplayName();
        _props.ExpandText = ExpandText;
        _props.CollapseText = CollapseText;
    }

    private ElementReference _root;
    private DotNetObjectReference<Ellipsis>? _obj;

    private bool _rendered = false;
    private CalcResult? _calcResult = null;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _obj = DotNetObjectReference.Create(this);
        }

        if (_calcResult == null)
        {
            Console.WriteLine($"_calcResult == null");
            _props.Expanded = _isExpanded;
            _calcResult = await JsRuntime.EllipsisCalcEllipsised(_obj!, _root, _props, Content);
            await InvokeStateHasChangedAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task HandleContentClick(MouseEventArgs arg)
    {
        if (OnContentClick.HasDelegate)
        {
            await OnContentClick.InvokeAsync();
        }
    }


    private async Task HandleCollapseClick(MouseEventArgs obj)
    {
        this._isExpanded = false;
        _calcResult = null;
        await InvokeStateHasChangedAsync();

    }

    private async Task HandleExpandClick(MouseEventArgs arg)
    {
        this._isExpanded = true;
        _calcResult = null;
        await InvokeStateHasChangedAsync();
    }


    protected override void Dispose(bool disposing)
    {
        _obj?.Dispose();
        base.Dispose(disposing);
    }
}
