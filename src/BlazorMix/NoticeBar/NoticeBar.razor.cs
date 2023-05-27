using BlazorMix.Core;

namespace BlazorMix;
public partial class NoticeBar
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-notice-bar";

    public static RenderFragment DefaultIcon = b => b.Fluent()
        .OpenComponent<SoundOutline>()
        .CloseComponent();

    public static RenderFragment DefaultCloseIcon = b => b.Fluent()
        .OpenComponent<CloseOutline>()
        .AddAttribute("Class", new ClassBuilder($"{ClsPrefix}-close-icon"))
        .CloseComponent();


    /// <summary>
    /// The type of the NoticeBar
    /// </summary>
    [Parameter]
    public NoticeBarColor Color { get; set; } = NoticeBarColor.Default;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public int DelayMs { get; set; } = 2000;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public int Speed { get; set; } = 50;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Closeable { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<Task>? OnClose { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? Extra { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? Icon { get; set; } = DefaultIcon;  
    
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment CloseIcon { get; set; } = DefaultCloseIcon;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; }


    protected override void OnInitialized()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Color.GetDisplayName()}")
            .AddIf($"{ClsPrefix}-wrap", () => Wrap)
            .Add(() => Class?.ToString() ?? "");
    }

    private ElementReference _contentEleRef;
    private StateFunc2 _scrollFunc;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            _scrollFunc = new StateFunc2(async () =>
            {
                await JsRuntime.NoticeBarEndScroll(
                    _contentEleRef
                    );
            }, async () =>
            {
                await JsRuntime.NoticeBarStartScroll(
                    _contentEleRef,
                    DelayMs < 0 ? 2000 : DelayMs,
                    Speed < 1 ? 50 : Speed
                    );
            });
        }

        if (!Wrap && !_scrollFunc.State)
        {
            await _scrollFunc.Invoke();
        }
    }


    private Task HandleTransitionEnd()
    {
        return Task.CompletedTask;
    }

    private bool _visible = true;
    
    private async Task HandleClose()
    {
        _visible = false;
        if (!Wrap && _scrollFunc.State)
        {
            await _scrollFunc.Invoke();
        }
        if (OnClose != null)
        {
            await OnClose();
        }
        StateHasChanged();
    }
}
