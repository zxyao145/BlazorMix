
namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Divider : BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-divider";

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

    protected override Task OnInitializedAsync()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{ContentPosition.GetDisplayName()}")
            .Add($"{ClsPrefix}-{Direction.GetDisplayName()}");
        return base.OnInitializedAsync();
    }
}
