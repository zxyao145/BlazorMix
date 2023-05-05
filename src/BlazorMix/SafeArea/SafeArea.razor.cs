using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;


/// <summary>
/// 
/// </summary>
public enum SafeAreaPosition : byte
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
}


public partial class SafeArea
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-safe-area";

    [Parameter]
    public SafeAreaPosition Position { get; set; }

    private readonly ClassBuilder _classBuilder = new();
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _classBuilder.Clear()
            .Add(PrefixCls)
            .Add($"{PrefixCls}-position-{Position.GetDisplayName()}");
    }
}
