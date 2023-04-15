using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public partial class SpaceItem
{
    public const string PrefixCls = "bm-space-item";

    [CascadingParameter]
    public Space Parent { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
