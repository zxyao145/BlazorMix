
namespace BlazorMix.Core;
public static class JsRuntimeEtx
{
    #region common

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

    public static async Task MoveEleTo(
        this IJSRuntime js,
        string selector,
        OneOf<string, ElementReference> container
    )
    {
        if (container.IsT0)
        {
            await js.InvokeVoidAsync(JsConstants.MoveEleTo, selector, container.AsT0);
        }
        else
        {
            await js.InvokeVoidAsync(JsConstants.MoveEleTo, selector, container.AsT1);
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
        await js.InvokeVoidAsync(JsConstants.EnableBodyScroll);
    }

    #endregion

    #region NoticeBar

    public static async Task NoticeBarStartScroll(
        this IJSRuntime js,
        ElementReference element,
        int delayMs,
        int speed
    )
    {
        await js.InvokeVoidAsync(JsConstants.NoticeBarStartScroll, element, delayMs, speed);
    }
    public static async Task NoticeBarEndScroll(
        this IJSRuntime js,
        ElementReference element
    )
    {
        await js.InvokeVoidAsync(JsConstants.NoticeBarEndScroll, element);
    }
    #endregion


    #region SwipeAction

    public static async Task SwipeActionInit(
        this IJSRuntime js,
        DotNetObjectReference<SwipeAction> obj,
        ElementReference componentRoot
    )
    {
        await js.InvokeVoidAsync(JsConstants.SwipeActionInit, obj, componentRoot);
    }

    public static async Task SwipeActionStop(
        this IJSRuntime js,
        DotNetObjectReference<SwipeAction> obj
    )
    {
        await js.InvokeVoidAsync(JsConstants.SwipeActionStop, obj);
    }
    #endregion
}
