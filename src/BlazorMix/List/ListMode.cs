using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

public enum ListMode : byte
{
    [Display(Name = "default")]
    Default = 0,

    [Display(Name = "card")]
    Card = 1,
}
