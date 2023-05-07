namespace BlazorMix;
public interface IActionSheetProps
{
    /// <summary>
    /// class
    /// </summary>
    [Parameter]
    public ClassBuilder? Class { get; set; }

    /// <summary>
    /// style
    /// </summary>
    [Parameter]
    public StyleBuilder? Style { get; set; }

    /// <summary>
    /// 面板选项列表
    /// </summary>
    [Parameter]
    public List<ActionInfo> Actions { get; set; }

    /// <summary>
    /// 在关闭后回调
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// 在打开后回调
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterShow { get; set; }

    /// <summary>
    /// 顶部的额外区域	
    /// </summary>
    [Parameter]
    public StringOrRenderFragment? Extra { get; set; }

    /// <summary>
    /// 取消按钮文字，如果设置为空则不显示取消按钮
    /// </summary>
    [Parameter]
    public StringOrRenderFragment? CancelText { get; set; }

    /// <summary>
    /// 点击选项时触发，禁用或加载状态下不会触发
    /// </summary>
    [Parameter]
    public Func<ActionInfo, int, ValueTask>? OnAction { get; set; }

    /// <summary>
    /// 关闭时触发	
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// 点击选项后是否关闭	
    /// </summary>
    [Parameter]
    public bool CloseOnAction { get; set; }


    /// <summary>
    /// 是否开启安全区适配	
    /// </summary>
    [Parameter]
    public bool SafeArea { get; set; }
    
    /// <summary>
    /// 是否展示蒙层	
    /// </summary>
    [Parameter]
    public bool ShowMask { get; set; }

    /// <summary>
    /// 点击蒙层触发
    /// </summary>
    [Parameter]
    public EventCallback OnMaskClick { get; set; }

    /// <summary>
    /// 点击背景蒙层后是否关闭
    /// </summary>
    [Parameter]
    public bool CloseOnMaskClick { get; set; }

    /// <summary>
    /// Mask class
    /// </summary>
    [Parameter]
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// Mask style
    /// </summary>
    [Parameter]
    public StyleBuilder? MaskStyle { get; set; }

    /// <summary>
    /// Popup Class
    /// </summary>
    [Parameter]
    public ClassBuilder PopupClass { get; set; }

    /// <summary>
    /// Popup Style
    /// </summary>
    [Parameter]
    public StyleBuilder PopupStyle { get; set; }

}
