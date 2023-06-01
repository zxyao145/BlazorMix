using System.ComponentModel.DataAnnotations;

namespace BlazorMix;
public enum ResultStatus : byte
{
    [Display(Name = "success")]
    Success,

    [Display(Name = "error")]
    Error,

    [Display(Name = "info")]
    Info,

    [Display(Name = "waiting")]
    Waiting,

    [Display(Name = "warning")]
    Warning,
}
