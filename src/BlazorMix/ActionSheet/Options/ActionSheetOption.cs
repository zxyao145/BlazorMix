namespace BlazorMix;
public class ActionSheetOption: IActionSheetProps
{
    internal bool Visible { get; set; }

    internal bool Added { get; set; }


    /// <inheritdoc />
    [Parameter]
    public ClassBuilder? Class { get; set; }

    /// <inheritdoc />
    [Parameter]
    public StyleBuilder? Style { get; set; }

    /// <inheritdoc />
    [Parameter]
    public List<ActionInfo> Actions { get; set; } = new List<ActionInfo>();

    /// <inheritdoc />
    [Parameter]
    public Func<ValueTask>? AfterClose { get; set; }

    /// <inheritdoc />
    [Parameter]
    public Func<ValueTask>? AfterShow { get; set; }

    /// <inheritdoc />
    [Parameter]
    public StringOrRenderFragment? Extra { get; set; }

    /// <inheritdoc />
    [Parameter]
    public StringOrRenderFragment? CancelText { get; set; }

    /// <inheritdoc />
    [Parameter]
    public Func<ActionInfo, int, ValueTask>? OnAction { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool CloseOnAction { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool SafeArea { get; set; }

    /// <inheritdoc />
    [Parameter]
    public ClassBuilder PopupClass { get; set; } = new();

    /// <inheritdoc />
    [Parameter]
    public StyleBuilder PopupStyle { get; set; } = new();


    /// <inheritdoc />
    [Parameter]
    public bool ShowMask { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public EventCallback OnMaskClick { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool CloseOnMaskClick { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public ClassBuilder? MaskClass { get; set; }

    /// <inheritdoc />
    [Parameter]
    public StyleBuilder? MaskStyle { get; set; }

}
