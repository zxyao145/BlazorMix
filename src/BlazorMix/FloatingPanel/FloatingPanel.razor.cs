using BlazorMix.Core;
using OneOf.Types;

namespace BlazorMix;
public partial class FloatingPanel : IAsyncDisposable
{
    public const string ClsPrefix = "bm-floating-panel";

    /// <summary>
    /// 可拖拽至哪些高度，单位为 px	
    /// </summary>
    [Parameter]
    public int[] Anchors { get; set; } = Array.Empty<int>();

    /// <summary>
    /// 是否会处理面板内容区域的拖拽事件，禁用后则只能拖拽头部区域	
    /// </summary>
    [Parameter]
    public bool HandleDraggingOfContent { get; set; } = true;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 当高度变化时触发
    /// </summary>
    [Parameter]
    public EventCallback<double> OnHeightChange { get; set; }


    private ElementReference _rootRef;
    private DotNetObjectReference<FloatingPanel>? _obj;
    private IJSObjectReference? _mirror;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _obj = DotNetObjectReference.Create(this);
            _mirror = await JsRuntime.FloatingPanelInit(_obj,
                _rootRef,
                HandleDraggingOfContent,
                Anchors,
                CallbackFactory.Create<double, Task>(async (height) =>
                {
                    if (OnHeightChange.HasDelegate)
                    {
                        await OnHeightChange.InvokeAsync(height);
                    }
                })
                );
        }
        await base.OnAfterRenderAsync(firstRender);
    }


    public async Task SetHeight(int height)
    {
        if (_mirror != null)
        {
            await _mirror.InvokeVoidAsync("setHeight", height, true);
        }
    }


    protected override void Dispose(bool disposing)
    {
        _obj?.Dispose();
        base.Dispose(disposing);
    }

    public async ValueTask DisposeAsync()
    {
        if (_mirror is not null)
        {
            await _mirror.InvokeVoidAsync("dispose");
            await _mirror.DisposeAsync();
        }
    }
}
