using System.Runtime.CompilerServices;

namespace BlazorMix;

public partial class Button : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-btn";

    #region options

    /// <summary>
    /// Is block
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Is disable
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Is in loading
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Auto show loading state. 
    /// It's will override <see cref="Loading" />
    /// </summary>
    [Parameter]
    public bool AutoLoading { get; set; }

    /// <summary>
    ///     <para>
    ///     The color of button. All built-in values please see <see cref="ButtonColor" />.
    ///     </para>
    ///     <para>
    ///     If you want to customize colors, please define class <c>.btn-{Color}</c>
    ///     </para>
    /// </summary>
    [Parameter]
    public string Color { get; set; } = ButtonColor.Default;

    /// <summary>
    /// The shape of the button
    /// </summary>
    [Parameter]
    public string Shape { get; set; } = ButtonShape.Rounded;

    /// <summary>
    /// The type attribute of the native button element
    /// </summary>
    [Parameter]
    public string Type { get; set; } = ButtonType.Button;

    /// <summary>
    ///Button size
    /// </summary>
    [Parameter]
    public string Size { get; set; } = ButtonSize.Medium;

    /// <summary>
    /// Fill model.
    /// </summary>
    [Parameter]
    public string Fill { get; set; } = ButtonFillType.Solid;

    /// <summary>
    /// Button internal icon, fully customizable. It will be hide when <see cref="Loading"/> is true.
    /// </summary>
    [Parameter]
    public RenderFragment? Icon { get; set; }

    /// <summary>
    /// loading icon
    /// </summary>
    /// <default>
    /// LoadingIcon
    /// </default>
    [Parameter]
    public RenderFragment LoadingIcon { get; set; } = ButtonOptions.DefaultLoadingIcon;

    /// <summary>
    /// OnClick event.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// button navite click event handler
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task HandleClick(MouseEventArgs e)
    {
        if (!Loading && !Disabled && OnClick.HasDelegate)
        {
            if (!AutoLoading)
            {
                await OnClick.InvokeAsync(e);
            }
            else
            {
                Loading = true;
                StateHasChanged();

                await OnClick.InvokeAsync(e);

                Loading = false;
                StateHasChanged();
            }
        }
    }

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
            .Add(PrefixCls)
            .Add($"{Shape}")
            .Add($"{PrefixCls}-{Color}")
            .AddIf($"{PrefixCls}-{Size}", () => Size != ButtonSize.Medium)
            .AddIf($"{PrefixCls}-{Fill}", () => Fill != ButtonFillType.Solid)
            .AddIf($"{PrefixCls}-block", () => Block)
            .AddIf($"{PrefixCls}-disabled", () => Disabled)
            .AddIf($"{PrefixCls}-loading", () => Loading);
    }
}
