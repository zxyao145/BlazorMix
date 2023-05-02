

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Popup : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-popup";

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #region paramters

    /// <summary>
    /// 是否可见	
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }


    private Position _pos = Position.Bottom;

    /// <summary>
    /// 指定弹出的位置	
    /// </summary>
    [Parameter]
    public Position Position
    {
        get => _pos;
        set
        {
            if (_pos == value)
            {
                return;
            }

            _pos = value;
            _aimateName = _pos switch
            {
                Position.Top => AniNames.FadeDown,
                Position.Left => AniNames.FadeLeft,
                Position.Right => AniNames.FadeRight,
                _ => AniNames.FadeUp
            };
        }
    } 

    /// <summary>
    /// 关闭时触发	
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// 点击时触发
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 是否显示关闭按钮
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; }

    /// <summary>
    /// 不可见时卸载内容
    /// </summary>
    [Parameter]
    public bool DestroyOnClose { get; set; } = true;


    #region Mask 

    /// <summary>
    /// 是否展示蒙层	
    /// </summary>
    [Parameter]
    public bool ShowMask { get; set; } = true;

    /// <summary>
    /// 点击蒙层触发
    /// </summary>
    [Parameter]
    public EventCallback OnMaskClick { get; set; }

    /// <summary>
    /// 点击背景蒙层后是否关闭
    /// </summary>
    [Parameter]
    public bool CloseOnMaskClick { get; set; } = true;

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

    #endregion

    #endregion

    private string _aimateName = AniNames.FadeUp;

    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _classBuilder
            .Add($"{PrefixCls}-body")
            .Add($"{PrefixCls}-body-position-{Position.GetDisplayName()}")
            .Add(() => Class?.ToString() ?? "");
    }


    private async Task HandleOnMaskClick()
    {
        if (OnMaskClick.HasDelegate)
        {
            await OnMaskClick.InvokeAsync();
        }

        if (CloseOnMaskClick)
        {
            Console.WriteLine("CloseOnMaskClick");
            {
                await OnClose.InvokeAsync();
            }
        }
    }

    private async Task HandleCloserClick(MouseEventArgs e)
    {
        if (OnClose.HasDelegate)
        {
            Console.WriteLine("HandleCloserClick");
            await OnClose.InvokeAsync();
        }
    }
}
