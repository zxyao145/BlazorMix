
using System.Collections.Generic;

namespace BlazorMix;

public partial class SpaceItem
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-space-item";

    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter, EditorRequired]
    public Space Parent { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (Parent == null)
        {
            throw new ArgumentNullException($"{nameof(SpaceItem)} must be a child of Space");
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
