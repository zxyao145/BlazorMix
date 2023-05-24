using BlazorMix.Core;

namespace BlazorMix;
public partial class SwipeAction
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
    public bool CloseOnTouchOutside { get; set; }

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

    private ElementReference _root;
    private DotNetObjectReference<SwipeAction>? obj;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            obj = DotNetObjectReference.Create(this);
            await JsRuntime.SwipeActionInit(obj, _root);
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
        await JsRuntime.SwipeActionStop(obj!);
    }

    [JSInvokable]
    public async Task HandleClose()
    {
        await Close();
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
        obj?.Dispose();
        base.Dispose(disposing);
    }
}
