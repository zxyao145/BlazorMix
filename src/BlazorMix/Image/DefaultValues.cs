using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
internal class ImageDefaultValues
{
    public static RenderFragment DefaultPlaceholder = b =>
        b.Fluent()
            .OpenElement("div")
            .AddAttribute($"{Image.ClsPrefix}-tip")
            .OpenComponent<ImageIcon>()
            .CloseComponent()
            .CloseElement();

    public static RenderFragment DefaultFallback = b =>
        b.Fluent()
            .OpenElement("div")
            .AddAttribute($"{Image.ClsPrefix}-tip")
            .OpenComponent<BrokenImageIcon>()
            .CloseComponent()
            .CloseElement();
}
