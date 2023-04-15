using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;


public class SpaceDirection
{
    public static string Vertical => "vertical";

    public static string Horizontal => "horizontal";
}

public partial class Space
{
    public const string PrefixCls = "bm-space";

    /// <summary>
    /// start | end |center |baseline
    /// </summary>
    [Parameter]
    public string Align { get; set; } = "";

    [Parameter]
    public string Justify { get; set; } = "";

    [Parameter]
    public bool Block { get; set; }

    [Parameter]
    public bool Wrap { get; set; }

    [Parameter]
    public string Direction { get; set; } = SpaceDirection.Horizontal;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        _classBuilder = new ClassBuilder();
        _classBuilder
            .Add(PrefixCls)
            .Add($"{PrefixCls}-{Direction}")
            .AddIf($"{PrefixCls}-block", () => Block)
            .AddIf($"{PrefixCls}-wrap", () => Wrap)
            .AddIf($"{PrefixCls}-justify-{Justify}", () => !string.IsNullOrWhiteSpace(Justify))
            .AddIf($"{PrefixCls}-align-{Align}", () => !string.IsNullOrWhiteSpace(Align))
            ;
    }
}
