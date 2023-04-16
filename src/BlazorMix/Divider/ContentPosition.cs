using System.ComponentModel.DataAnnotations;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public enum ContentPosition : byte
{
    /// <summary>
    /// center
    /// </summary>
    [Display(Name = "center")]
    Center = 0,

    /// <summary>
    /// left
    /// </summary>
    [Display(Name = "left")]
    Left = 1,

    /// <summary>
    /// right
    /// </summary>
    [Display(Name = "right")]
    Right = 1,
}
