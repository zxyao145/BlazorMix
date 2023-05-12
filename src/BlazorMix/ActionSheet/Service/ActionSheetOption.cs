namespace BlazorMix;
public class ActionSheetOption : IActionSheetProps
{
    internal bool Visible { get; set; }

    internal bool Added { get; set; }


    /// <inheritdoc />
    public ClassBuilder? Class { get; set; }

    /// <inheritdoc />
    public StyleBuilder? Style { get; set; }

    /// <inheritdoc />
    public List<ActionInfo> Actions { get; set; } = new List<ActionInfo>();

    /// <inheritdoc />
    public Func<ValueTask>? AfterClose { get; set; }

    /// <inheritdoc />
    public Func<ValueTask>? AfterShow { get; set; }

    /// <inheritdoc />
    public StringOrRenderFragment? Extra { get; set; }

    /// <inheritdoc />
    public StringOrRenderFragment? CancelText { get; set; }

    /// <inheritdoc />
    public Func<ActionInfo, int, ValueTask>? OnAction { get; set; }

    /// <inheritdoc />
    public EventCallback OnClose { get; set; }

    /// <inheritdoc />
    public bool CloseOnAction { get; set; }

    /// <inheritdoc />
    public bool SafeArea { get; set; }

    /// <inheritdoc />
    public ClassBuilder PopupClass { get; set; } = new();

    /// <inheritdoc />
    public StyleBuilder PopupStyle { get; set; } = new();


    /// <inheritdoc />
    public bool ShowMask { get; set; } = true;

    /// <inheritdoc />
    public EventCallback OnMaskClick { get; set; }

    /// <inheritdoc />
    public bool CloseOnMaskClick { get; set; } = true;

    /// <inheritdoc />
    public ClassBuilder? MaskClass { get; set; }

    /// <inheritdoc />
    public StyleBuilder? MaskStyle { get; set; }

}
