namespace BlazorMix;

public class DialogOption
{
    internal bool Visible { get; set; }

    internal bool Added { get; set; }

    internal bool DestroyOnClose { get;  } = true;

    public ClassBuilder Class { get; set; } = new();
    public StyleBuilder Style { get; set; } = new();

    #region CenterPopup

    /// <summary>
    /// 在关闭后回调
    /// </summary>
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// 在打开后回调
    /// </summary>
    public Func<ValueTask>? AfterShow { get; set; }

    /// <summary>
    /// 点击背景蒙层后是否关闭
    /// </summary>
    public bool CloseOnMaskClick { get; set; } = false;

    /// <summary>
    /// Dialog 遮罩类名	
    /// </summary>
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// Dialog 遮罩样式	
    /// </summary>
    public StyleBuilder? MaskStyle { get; set; }


    /// <summary>
    /// 指定挂载的 HTML 节点，如果为 null 的话，会渲染到当前节点.
    /// </summary>
    internal string? Container { get; set; } = null;

    /// <summary>
    /// Dialog 内容类名
    /// </summary>
    public ClassBuilder? BodyClass { get; set; }

    /// <summary>
    /// Dialog 内容样式
    /// </summary>
    public StyleBuilder? BodyStyle { get; set; }


    /// <summary>
    /// 是否禁用 body 滚动
    /// </summary>
    public bool DisableBodyScroll { get; set; } = true;

    #endregion

    /// <summary>
    /// alert 模式中不允许设置
    /// </summary>
    public bool CloseOnAction { get; set; }

    /// <summary>
    /// 操作按钮列表，可以传入二级数组（List&lt;DialogActionItem&gt;）来实现同一行内并排多个按钮。
    /// Alert 和 Confirm 模式中不允许设置
    /// </summary>
    public List<DialogAction> Actions { get; set; } = new();

    /// <summary>
    /// 图片 url	
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// 顶部区域	
    /// </summary>
    public StringOrRenderFragment? Header { get; set; }

    /// <summary>
    /// 对话框标题	
    /// </summary>
    public StringOrRenderFragment? Title { get; set; }

    /// <summary>
    /// 对话框内容	
    /// </summary>
    public StringOrRenderFragment? Content { get; set; }

    /// <summary>
    /// 点击操作按钮时触发	
    /// </summary>
    public Func<DialogActionItem, int, Task>? OnAction { get; set; }

    /// <summary>
    /// 关闭时触发	
    /// </summary>
    public Func<Task>? OnClose { get; set; }
}
