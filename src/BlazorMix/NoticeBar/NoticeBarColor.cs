using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace BlazorMix;

public enum NoticeBarColor: byte
{
    [Display(Name = "default")]
    Default,

    // warning
    [Display(Name = "alert")]
    Alert,   
    
    [Display(Name = "info")]
    Info,  
    
    [Display(Name = "error")]
    Error,

    //public static string Default => "default";

    //public static string Alert => "alert";

    //public static string Info => "info";

    //public static string Error => "error";
}
