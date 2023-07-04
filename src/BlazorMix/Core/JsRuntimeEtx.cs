
using OneOf.Types;

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

    public static async Task ObserveInViewportOnce(
    this IJSRuntime js,
        DotNetObjectReference<CallbackFunc<Task>> dotNetObject,
        ElementReference element
        )
    {
        await js.InvokeVoidAsync(JsConstants.ObserveInViewportOnce, dotNetObject, element);
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
        return await js.InvokeAsync<IJSObjectReference>(JsConstants.SwipeActionInit, obj, closeOnTouchOutside, componentRoot);
    }


    #endregion

    #region Ellipsised

    public static async Task<CalcResult> EllipsisCalcEllipsised(
        this IJSRuntime js,
        DotNetObjectReference<Ellipsis> obj,
        ElementReference componentRoot,
        EllipsisProps props,
        string content
    )
    {
        return await js.InvokeAsync<CalcResult>(
            JsConstants.EllipsisCalcEllipsised,
            obj,
            componentRoot,
            props,
            content
            );
    }

    #endregion


    #region WaterMark

    public static async Task WaterMarkRender(
        this IJSRuntime js,
        ElementReference componentRoot,
        WaterMarkProps props
    )
    {
        await js.InvokeVoidAsync(
            JsConstants.PrefixWaterMarkRender,
            componentRoot,
            props
            );
    }

    #endregion


    #region FloatingPanel

    /// <summary>
    /// 
    /// </summary>
    /// <param name="js"></param>
    /// <param name="obj"></param>
    /// <param name="root"></param>
    /// <param name="handleDraggingOfContent"></param>
    /// <param name="anchors"></param>
    /// <param name="callbackDotNetObject">
    ///  on height change callback
    /// </param>
    /// <returns></returns>
    internal static async Task<IJSObjectReference> FloatingPanelInit(
        this IJSRuntime js,
        DotNetObjectReference<FloatingPanel> obj,
        ElementReference root,
        bool handleDraggingOfContent,
        int[] anchors,
        DotNetObjectReference<CallbackFunc<double, Task>> callbackDotNetObject
    )
    {
        return await js.InvokeAsync<IJSObjectReference>(
            JsConstants.FloatingPanelInit,
            obj,
            root,
            handleDraggingOfContent,
            anchors,
            callbackDotNetObject
            );
    }

    #endregion
}
