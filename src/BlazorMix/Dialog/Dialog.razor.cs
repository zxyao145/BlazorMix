
using System;

namespace BlazorMix;


public partial class Dialog
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-dialog";

    #region CenterPopup

    /// <summary>
    /// 显示隐藏	
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

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
    /// 点击背景蒙层后是否关闭
    /// </summary>
    [Parameter]
    public bool CloseOnMaskClick { get; set; } = false;

    /// <summary>
    /// Dialog 遮罩类名	
    /// </summary>
    [Parameter]
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// Dialog 遮罩样式	
    /// </summary>
    [Parameter]
    public StyleBuilder? MaskStyle { get; set; }

    /// <summary>
    /// 指定挂载的 HTML 节点，如果为 null 的话，会渲染到当前节点.
    /// </summary>
    [Parameter]
    public string Container { get; set; } = "body";

    /// <summary>
    /// Dialog 内容类名
    /// </summary>
    [Parameter]
    public ClassBuilder? BodyClass { get; set; }

    /// <summary>
    /// Dialog 内容样式
    /// </summary>
    [Parameter]
    public StyleBuilder? BodyStyle { get; set; }

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

    #endregion

    /// <summary>
    /// 操作按钮列表，可以传入二级数组（List&lt;DialogActionItem&gt;）来实现同一行内并排多个按钮。
    /// Alert 和 Confirm 模式中不允许设置
    /// </summary>
    [Parameter]
    public List<DialogAction> Actions { get; set; } = new();

    /// <summary>
    /// 图片 url
    /// </summary>
    [Parameter]
    public string? Image { get; set; }

    /// <summary>
    /// 顶部区域	
    /// </summary>
    [Parameter]
    public StringOrRenderFragment? Header { get; set; }

    /// <summary>
    /// 对话框标题	
    /// </summary>
    [Parameter]
    public StringOrRenderFragment? Title { get; set; }

    /// <summary>
    /// 对话框内容	
    /// </summary>
    [Parameter]
    public StringOrRenderFragment? Content { get; set; }

    /// <summary>
    /// Content 的代理
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent
    {
        get => Content?.Node;
        set
        {
            Content = value ?? null;
        }
    }

    /// <summary>
    /// 点击操作按钮时触发	
    /// </summary>
    [Parameter]
    public Func<DialogActionItem, int, Task>? OnAction { get; set; }

    /// <summary>
    /// 点击操作按钮后后是否关闭	
    /// </summary>
    [Parameter]
    public bool CloseOnAction { get; set; }

    /// <summary>
    /// 关闭时触发
    /// </summary>
    [Parameter]
    public EventCallback<DialogActionEventArgs> OnClose { get; set; }

    private ClassBuilder _bodyClass = new();
    protected override Task OnInitializedAsync()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            ;
        _bodyClass
            .Add($"{ClsPrefix}-body")
            .AddIf($"{ClsPrefix}-with-image", () => !string.IsNullOrWhiteSpace(Image))
            .Add(() => BodyClass?.ToString() ?? "");

        return base.OnInitializedAsync();
    }

    private async Task HandleAction(DialogActionItem actionItem, int index)
    {
        var t1 = Task.CompletedTask;
        var e = new DialogActionEventArgs();
        if (actionItem.OnClick!= null)
        {
            t1 = actionItem.OnClick(e);
        }
        var t2 = OnAction?.Invoke(actionItem, index) ?? Task.CompletedTask;

        await Task.WhenAll(t1, t2);

        if (!e.Cancel && CloseOnAction && OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    private async Task HandleMaskClick()
    {
        if (CloseOnMaskClick && OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }
}
