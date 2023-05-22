namespace BlazorMix;
public interface IActionSheetProps
{
    /// <summary>
    /// class
    /// </summary>
    public ClassBuilder? Class { get; set; }

    /// <summary>
    /// style
    /// </summary>
    public StyleBuilder? Style { get; set; }

    /// <summary>
    /// 面板选项列表
    /// </summary>
    public List<ActionInfo> Actions { get; set; }

    /// <summary>
    /// 在关闭后回调
    /// </summary>
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// 在打开后回调
    /// </summary>
    public Func<ValueTask>? AfterShow { get; set; }

    /// <summary>
    /// 顶部的额外区域	
    /// </summary>
    public StringOrRenderFragment? Extra { get; set; }

    /// <summary>
    /// 取消按钮文字，如果设置为空则不显示取消按钮
    /// </summary>
    public StringOrRenderFragment? CancelText { get; set; }

    /// <summary>
    /// 点击选项时触发，禁用或加载状态下不会触发
    /// </summary>
    public Func<ActionInfo, int, ValueTask>? OnAction { get; set; }

    /// <summary>
    /// 关闭时触发	
    /// </summary>
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// 点击选项后是否关闭	
    /// </summary>
    public bool CloseOnAction { get; set; }


    /// <summary>
    /// 是否开启安全区适配	
    /// </summary>
    public bool SafeArea { get; set; }
    
    /// <summary>
    /// 是否展示蒙层	
    /// </summary>
    public bool ShowMask { get; set; }

    /// <summary>
    /// 点击蒙层触发
    /// </summary>
    public EventCallback OnMaskClick { get; set; }

    /// <summary>
    /// 点击背景蒙层后是否关闭
    /// </summary>
    public bool CloseOnMaskClick { get; set; }

    /// <summary>
    /// Mask class
    /// </summary>
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// Mask style
    /// </summary>
    public StyleBuilder? MaskStyle { get; set; }

    /// <summary>
    /// Popup Class
    /// </summary>
    public ClassBuilder PopupClass { get; set; }

    /// <summary>
    /// Popup Style
    /// </summary>
    public StyleBuilder PopupStyle { get; set; }

}
