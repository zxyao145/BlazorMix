
using System.ComponentModel.DataAnnotations;

namespace BlazorMix;
public enum SwipeActionColor : byte
{
    [Display(Name = "light")]
    Light,

    [Display(Name = "weak")]
    Weak,

    [Display(Name = "primary")]
    Primary,

    [Display(Name = "success")]
    Success,

    [Display(Name = "warning")]
    Warning,

    [Display(Name = "danger")]
    Danger,
}


public enum SwipeActionDirection : byte
{
    [Display(Name = "left")]
    Left,

    [Display(Name = "right")]
    Right,
}

