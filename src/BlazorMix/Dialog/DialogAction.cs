namespace BlazorMix;


public class DialogAction
{
    internal List<DialogActionItem> ActionList { get; set; } = new();

    private DialogActionItem? _firstActionItem = null;

    protected DialogActionItem FirstActionItem
    {
        get
        {
            if (_firstActionItem != null)
            {
                return _firstActionItem;
            }

            _firstActionItem = new DialogActionItem();
            ActionList.Clear();
            ActionList.Add(_firstActionItem);

            return _firstActionItem;
        }
    }

    public DialogAction()
    {

    }

    internal DialogAction(DialogActionItem? actionInfo)
    {
        if (actionInfo != null)
        {
            ActionList.Add(actionInfo);
        }
    }

    internal DialogAction(List<DialogActionItem>? action)
    {
        if (action != null)
        {
            ActionList = action;
        }
    }

    #region paramter

    /// <summary>
    /// 唯一标记	
    /// </summary>
    public string Key
    {
        get => FirstActionItem.Key;
        set => FirstActionItem.Key =value;
    } 

    /// <summary>
    /// 标题	
    /// </summary>
    public StringOrRenderFragment Text
    {
        get => FirstActionItem.Text;
        set => FirstActionItem.Text = value;
    }

    /// <summary>
    /// 是否为禁用状态	
    /// </summary>
    public bool Disabled
    {
        get => FirstActionItem.Disabled;
        set => FirstActionItem.Disabled = value;
    }

    /// <summary>
    /// 是否为危险状态
    /// </summary>
    public bool Danger
    {
        get => FirstActionItem.Danger;
        set => FirstActionItem.Danger = value;
    }

    /// <summary>
    /// 是否文字加粗	
    /// </summary>
    public bool Bold
    {
        get => FirstActionItem.Bold;
        set => FirstActionItem.Bold = value;
    }

    /// <summary>
    /// Action 类名
    /// </summary>
    public ClassBuilder? Class
    {
        get => FirstActionItem.Class;
        set => FirstActionItem.Class = value;
    }

    /// <summary>
    /// Action 样式
    /// </summary>
    public StyleBuilder? Style
    {
        get => FirstActionItem.Style;
        set => FirstActionItem.Style = value;
    }

    /// <summary>
    /// 点击时触发	
    /// </summary>
    public Func<DialogActionEventArgs, Task>? OnClick
    {
        get => FirstActionItem.OnClick;
        set => FirstActionItem.OnClick = value;
    }

    #endregion

    public static implicit operator DialogAction(DialogActionItem actionItem) => new(actionItem);

    public static implicit operator DialogAction(List<DialogActionItem> actionInfos) => new(actionInfos);
}

/// <summary>
/// DialogActionItem / List&lt;DialogActionItem&gt; 可以隐式转换为 DialogAction
/// </summary>
public class DialogActionItem
{
    /// <summary>
    /// 唯一标记	
    /// </summary>
    public string Key { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 标题	
    /// </summary>
    public StringOrRenderFragment Text { get; set; } = "";

    /// <summary>
    /// 是否为禁用状态	
    /// </summary>
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// 是否为危险状态
    /// </summary>
    public bool Danger { get; set; } = false;

    /// <summary>
    /// 是否文字加粗	
    /// </summary>
    public bool Bold { get; set; } = false;

    /// <summary>
    /// Action 类名
    /// </summary>
    public ClassBuilder? Class { get; set; }

    /// <summary>
    /// Action 样式
    /// </summary>
    public StyleBuilder? Style { get; set; }

    /// <summary>
    /// 按钮点击时触发	
    /// </summary>
    public Func<DialogActionEventArgs, Task>? OnClick { get; set; }
}