namespace BlazorMix;

public abstract class BmJsDomComponentBase : BmDomComponentBase
{
    [Inject]
    protected IJSRuntime Js { get; set; } = default!;

    protected async Task<T> JsInvokeAsync<T>(string identifier, params object[] args)
    {
        return await Js.InvokeAsync<T>(identifier, args);
    }

    protected async Task JsInvokeAsync(string identifier, params object[] args)
    {
        await Js.InvokeVoidAsync(identifier, args);
    }
}
