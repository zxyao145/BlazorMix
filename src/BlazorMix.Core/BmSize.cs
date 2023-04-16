using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

public enum BmSize: byte
{
    [Display(Name = "md")]
    Medium = 0,

    [Display(Name = "xs")]
    Mini = 1,

    [Display(Name = "sm")]
    Small =2,

    [Display(Name = "lg")]
    Large = 3,

    [Display(Name = "xl")]
    ExtraLarge = 4,
}