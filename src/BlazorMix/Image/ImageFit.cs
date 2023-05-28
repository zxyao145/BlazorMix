using System.ComponentModel.DataAnnotations;

namespace BlazorMix;
public enum ImageFit : byte
{
    [Display(Name = "fill")]
    Fill = 0,

    [Display(Name = "none")]
    None = 1,

    [Display(Name = "contain")]
    Contain = 2,

    [Display(Name = "cover")]
    Cover = 3,

    [Display(Name = "scale-down")]
    ScaleDown = 4
}
