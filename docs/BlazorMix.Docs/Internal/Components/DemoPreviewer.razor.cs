using BlazorMix.Docs.Core.Services;
using BlazorMix.Docs.Internal.Attribute;
using Microsoft.AspNetCore.Components;

namespace BlazorMix.Docs.Internal.Components;

[JsCustomElement("code-demo")]
public partial class DemoPreviewer
{
    [Inject]
    private DocService DocService { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Src { get; set; } = null!;


    private string _sourceCode = "";


    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        ArgumentNullException.ThrowIfNull(Src);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await CalcState();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CalcState()
    {
        var src = Src.StartsWith("/") ? Src.Substring(1) : Src;
        var sourceCode = await DocService.ReadExampleAsync(src);
        var (razor, cs, css) = ResolveSourceCode(sourceCode);
        _sourceCode = razor ?? "";
    }


    private static (string? Razor, string? cs, string? css) ResolveSourceCode(string sourceCode)
    {
        string? razor = null;
        string? cs = null;
        string? css = null;

        var code = sourceCode;
        var cssFrom = sourceCode.IndexOf("<style", StringComparison.Ordinal);
        var cssTo = sourceCode.IndexOf("</style>", StringComparison.Ordinal) + "</style>".Length;

        if (cssFrom > -1 && cssTo > -1)
        {
            var cssContent = sourceCode.Substring(cssFrom, cssTo - cssFrom);
            css = cssContent;

            code = code.Replace(cssContent, "");
        }

        var codeIndex = code.IndexOf("@code");
        if (codeIndex > -1)
        {
            razor = code.Substring(0, codeIndex).Trim();
            cs = code.Substring(codeIndex).Trim();
        }
        else
        {
            razor = code.Trim();
        }

        return (razor, cs, css);
    }
}
