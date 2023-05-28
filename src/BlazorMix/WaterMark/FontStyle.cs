using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public enum FontStyle: byte
{
    [Display(Name = "normal")]
    Normal = 0,

    [Display(Name = "none")]
    None = 1,

    [Display(Name = "italic")]
    Italic = 2,

    [Display(Name = "oblique")]
    Oblique = 3,
}
