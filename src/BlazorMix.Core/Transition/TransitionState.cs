using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;

#pragma warning disable CS1591
public enum TransitionState
{
    [Display(Name = "unmounted")]
    Unmounted,

    [Display(Name = "enter")]
    Enter,

    [Display(Name = "entering")]
    Entering,

    [Display(Name = "entered")]
    Entered,

    [Display(Name = "exit")]
    Leave,

    [Display(Name = "exiting")]
    Leaving,

    [Display(Name = "exited")]
    Leaved,
}
