namespace BlazorMix;
public class ToastOption
{
    internal static int GlobalDurationMs = 2000;
    internal static ToastPosition GlobalPosition = ToastPosition.Center;
    internal static bool GlobalMaskClickable = true;

    internal bool Visible { get; set; }

    internal Task OnMaskClick()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Toast 完全关闭后的回调
    /// </summary>
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// Toast 文本内容
    /// </summary>
    public StringOrRenderFragment Content { get; set; }

    /// <summary>
    /// 提示持续时间，若小于等于 0 则不会自动关闭
    /// </summary>
    public int DurationMs { get; set; } = GlobalDurationMs;

    /// <summary>
    /// Toast 图标，支持枚举 ToastIconType 或 RenderFragment
    /// </summary> 
    public OneOf<ToastIconType, RenderFragment>? Icon { get; set; }

    /// <summary>
    /// Toast 遮罩类名
    /// </summary>
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// 是否允许背景点击
    /// </summary>
    public bool MaskClickable { get; set; } = GlobalMaskClickable;

    /// <summary>
    /// Toast 遮罩样式
    /// </summary>
    public StyleBuilder? MaskStyle { get; set; }

    /// <summary>
    /// 垂直方向显示位置	
    /// </summary>
    public ToastPosition Position { get; set; } = GlobalPosition;
}
