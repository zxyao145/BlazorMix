namespace BlazorMix;
public abstract class MaskBase: BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-mask";

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    #region paramters

    /// <summary>
    /// 完全关闭后触发
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// 完全展示后触发
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterShow { get; set; }

    /// <summary>
    /// 点击蒙层自身触发
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 背景蒙层的颜色，"black" | "white"
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "black";

    /// <summary>
    /// 透明度
    /// </summary>
    [Parameter]
    public double Opacity { get; set; } = 0.55;

    /// <summary>
    /// 不可见时卸载内容
    /// </summary>
    [Parameter]
    public bool DestroyOnClose { get; set; } = false;

    /// <summary>
    /// 是否禁用 body 滚动
    /// </summary>
    [Parameter]
    public bool DisableBodyScroll { get; set; } = true;

    /// <summary>
    /// 指定挂载的 HTML 节点，如果为 null 的话，会渲染到当前节点.
    /// </summary>
    [Parameter]
    public string Container { get; set; } = "body";

    /// <summary>
    /// 控制 Mask 显示还是关闭
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    #endregion
}
