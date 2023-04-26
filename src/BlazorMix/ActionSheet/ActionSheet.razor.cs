
namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class ActionSheet : BmDomComponentBase
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
    /// The position of the content. Only take effect when direction is <c>BmDirection.Horizontal</c>.
    /// </summary>
    [Parameter]
    public ContentPosition ContentPosition { get; set; }

    /// <summary>
    /// The direction type of divider
    /// </summary>
    [Parameter]
    public BmDirection Direction { get; set; }

    #endregion

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
            .Add($"{PrefixCls}-{ContentPosition.GetDisplayName()}")
            .Add($"{PrefixCls}-{Direction.GetDisplayName()}");
    }
}
