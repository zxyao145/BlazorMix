
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

    public static async Task<IJSObjectReference> SwipeActionInit(
        this IJSRuntime js,
        DotNetObjectReference<SwipeAction> obj,
        bool closeOnTouchOutside,
        ElementReference componentRoot
    )
    {
        return await js.InvokeAsync< IJSObjectReference>(JsConstants.SwipeActionInit, obj, closeOnTouchOutside, componentRoot);
    }

    #endregion
}
