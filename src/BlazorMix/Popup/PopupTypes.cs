
using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public enum Position : byte
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "bottom")]
    Bottom = 0,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "top")]
    Top = 1,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name ="left")]
    Left = 2,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "right")]
    Right = 3
}
