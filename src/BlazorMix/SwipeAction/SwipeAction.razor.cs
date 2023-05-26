using BlazorMix.Core;

namespace BlazorMix;
public partial class SwipeAction: IAsyncDisposable
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-swipe-action";

    [Parameter]
    public List<SwipeActionInfo> RightActions { get; set; } = new();

    [Parameter]
    public List<SwipeActionInfo> LeftActions { get; set; } = new();

    [Parameter]
    public EventCallback<SwipeActionInfo> OnAction { get; set; }

    [Parameter]
    public bool CloseOnTouchOutside { get; set; } = true;

    [Parameter]
    public bool CloseOnAction { get; set; } = true;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<SwipeActionDirection> OnActionsReveal { get; set; }

    protected override void OnInitialized()
    {
        _classBuilder
            .Clear()
            .Add(ClsPrefix)
            .Add(() => Class?.ToString() ?? "");
        base.OnInitialized();
    }


    protected override async Task OnParametersSetAsync()
    {
        bool lastCloseOnTouchOutside = CloseOnTouchOutside;
        await base.OnParametersSetAsync();

        if (_mirror is not null && lastCloseOnTouchOutside != CloseOnTouchOutside)
        {
            await _mirror.InvokeVoidAsync("setCloseOnTouchOutside", CloseOnTouchOutside).AsTask();
        }
    }


    private ElementReference _root;
    private DotNetObjectReference<SwipeAction>? _obj;
    private IJSObjectReference? _mirror;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _obj = DotNetObjectReference.Create(this);
            _mirror = await JsRuntime.SwipeActionInit(_obj, CloseOnTouchOutside, _root);
        }
        await base.OnAfterRenderAsync(firstRender);
    }


    public async Task HandleActionButtonClick(SwipeActionInfo action)
    {
        if (CloseOnAction)
        {
            await Close();
        }
        if (action.OnClick != null)
        {
            await action.OnClick();
        }

        if (OnAction.HasDelegate)
        {
            await OnAction.InvokeAsync(action);
        }
    }

    public async Task Close()
    {
        if (_mirror is not null)
        {
            await _mirror.InvokeVoidAsync(JsConstants.SwipeActionClose, CloseOnTouchOutside).AsTask();
        }
    }

    [JSInvokable]
    public async Task HandleActionsReveal(string leftOrRight)
    {
        if (OnActionsReveal.HasDelegate)
        {
            var dir = leftOrRight switch
            {
                "left" => SwipeActionDirection.Left,
                _ => SwipeActionDirection.Right,
            };
            await OnActionsReveal.InvokeAsync(dir);
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
            await _mirror.DisposeAsync();
        }
    }
}
