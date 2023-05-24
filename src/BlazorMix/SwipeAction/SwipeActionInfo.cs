using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public class SwipeActionInfo
{
    public string Key { get; set; } = Guid.NewGuid().ToString();

    public StringOrRenderFragment Text { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    public SwipeActionColor Color { get; set; }

    public Func<Task>? OnClick { get; set; }
}
