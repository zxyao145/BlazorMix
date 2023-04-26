
using System;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Popup : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-actionsheet";

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #region paramters

    /// <summary>
    /// 
    /// </summary>
    public EventCallback AfterClose { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public EventCallback AfterShow { get; set; }



//closeOnMaskClick?: boolean
//destroyOnClose?: boolean
//disableBodyScroll?: boolean
//forceRender?: boolean
//getContainer?: GetContainer
//mask?: boolean
//maskClassName?: string
//maskStyle?: MaskProps['style']
//onClick?: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void
//onClose?: () => void
//onMaskClick?: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void
//showCloseButton?: boolean
//stopPropagation?: PropagationEvent[]
//visible?: boolean


    #endregion

    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        _classBuilder.Clear()
            .Add(PrefixCls);
    }
}
