
using System.ComponentModel;
using System.Xml.Linq;

namespace BlazorMix.Core;
public static class JsRuntimeEtx
{
    public static async Task MoveEleTo(
        this IJSRuntime js,
        ElementReference element,
        OneOf<string, ElementReference> container
        )
    {
        if (container.IsT0)
        {
            await js.InvokeVoidAsync(JsConstants.MoveEleTo, element, container.AsT0);
        }
        else
        {
            await js.InvokeVoidAsync(JsConstants.MoveEleTo, element, container.AsT1);
        }
    }

    public static async Task DisableBodyScroll(
        this IJSRuntime js
    )
    {
        await js.InvokeVoidAsync(JsConstants.DisableBodyScroll);
    }

    public static async Task EnableBodyScroll(
        this IJSRuntime js
    )
    {
        await js.InvokeVoidAsync(JsConstants.DisableBodyScroll);
    }

}
