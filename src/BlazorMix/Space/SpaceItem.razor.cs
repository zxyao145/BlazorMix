
namespace BlazorMix;
public partial class SpaceItem
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-space-item";

    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    public Space Parent { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
