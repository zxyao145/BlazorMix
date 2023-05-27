using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

public enum EllipsisDirection : byte
{
    [Display(Name = "end")]
    End = 0,

    [Display(Name = "middle")]
    Middle = 1,

    [Display(Name = "start")]
    Start = 2,
}
