
using System.ComponentModel.DataAnnotations;

namespace BlazorMix;
public enum BmDirection: byte
{
    [Display(Name = "horizontal")]
    Horizontal = 0,


    [Display(Name = "vertical")]
    Vertical = 1,
}
