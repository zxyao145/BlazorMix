
namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class ActionSheet : BmDomComponentBase, IActionSheetProps
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-action-sheet";

    #region paramters

    /// <summary>
    /// 是否可见	
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// 不可见时卸载内容
    /// </summary>
    [Parameter]
    public bool DestroyOnClose { get; set; } = true;


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


    #region Mask 

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

    #endregion

    #endregion

    private readonly ClassBuilder _popupClass = new();


    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        _classBuilder
            .Clear()
            .Add(ClsPrefix);

        _popupClass.Clear()
            .Add(ClsPrefix + "-popup-body")
            .Add(() => PopupClass.ToString());
    }

    private async Task HandlePopupMaskClick()
    {
        if (OnMaskClick.HasDelegate)
        {
            await OnMaskClick.InvokeAsync();
        }

        if (CloseOnMaskClick && OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }
}
