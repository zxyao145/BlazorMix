
namespace BlazorMix;

public static class IJSRuntimeExtensions
{
    /// <summary>
    /// Determines hosting environment is WebAssembly.
    /// </summary>
    public static bool IsWebAssembly(this IJSRuntime js) => js is IJSInProcessRuntime;

    /// <summary>
    /// Asynchronously import specified javascript.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="path">
    /// The path of javascript file to import. Such as <c>./js/app.js</c> in wwwroot path.
    /// </param>
    /// <returns>A <see cref="ValueTask{TResult}"/> containing dynamic reference object of javascript.</returns>
    public static async ValueTask<dynamic> ImportAsync(this IJSRuntime js, string path)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", path);
        return new DynamicJsReferenceObject(module);
    }
}
