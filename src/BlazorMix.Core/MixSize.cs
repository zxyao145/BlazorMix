using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

public enum MixSize
{
    [Display(Name = "xs")]
    Mini,

    [Display(Name = "sm")]
    Small,

    [Display(Name = "md")]
    Medium,

    [Display(Name = "lg")]
    Large,

    [Display(Name = "xl")]
    ExtraLarge,
}