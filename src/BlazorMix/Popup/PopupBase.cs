
using BlazorMix.Core;

namespace BlazorMix;

public abstract class PopupBase: BmDomComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #region paramters

    /// <summary>
    /// 是否可见	
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// 在关闭后回调
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterClose { get; set; }

    /// <summary>
    /// 在打开后回调
    /// </summary>
    [Parameter]
    public Func<ValueTask>? AfterShow { get; set; }


    /// <summary>
    /// 关闭时触发	
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// 点击时触发
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 是否显示关闭按钮
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; }

    /// <summary>
    /// 是否禁用 body 滚动
    /// </summary>
    [Parameter]
    public bool DisableBodyScroll { get; set; } = true;

    /// <summary>
    /// 不可见时卸载内容
    /// </summary>
    [Parameter]
    public bool DestroyOnClose { get; set; } = true;

    /// <summary>
    /// 指定挂载的 HTML 节点，如果为 null 的话，会渲染到当前节点.
    /// </summary>
    [Parameter]
    public string Container { get; set; } = "body";

    /// <summary>
    /// body class
    /// </summary>
    [Parameter]
    public ClassBuilder BodyClass { get; set; } = new();

    /// <summary>
    /// body style
    /// </summary>
    [Parameter]
    public StyleBuilder BodyStyle { get; set; } = new();

    #region Mask 

    /// <summary>
    /// 是否展示蒙层	
    /// </summary>
    [Parameter]
    public bool ShowMask { get; set; } = true;

    /// <summary>
    /// 点击蒙层触发
    /// </summary>
    [Parameter]
    public EventCallback OnMaskClick { get; set; }

    /// <summary>
    /// 点击背景蒙层后是否关闭
    /// </summary>
    [Parameter]
    public bool CloseOnMaskClick { get; set; } = true;

    /// <summary>
    /// Mask class
    /// </summary>
    [Parameter]
    public ClassBuilder? MaskClass { get; set; }

    /// <summary>
    /// Mask style
    /// </summary>
    [Parameter]
    public StyleBuilder? MaskStyle { get; set; }

    #endregion

    #endregion

    protected ElementReference Ref;

    private StateFunc2? _afterCallbackState;
    private StateFunc? _moveEleToState;
    private StateFunc2? _disableBodyScrollState;
    protected bool _hasDestroyed = true;


    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var lastVisible = Visible;
        await base.SetParametersAsync(parameters);
        if (Visible)
        {
            _hasDestroyed = false;
        }

        _afterCallbackState ??= new StateFunc2(async () =>
        {
            if (AfterShow != null)
            {
                await AfterShow();
            }
        }, async () =>
        {
            if (AfterClose != null)
            {
                await AfterClose();
            }
        });
        if (lastVisible != Visible)
        {
            _afterCallbackState.State = Visible;
        }
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _moveEleToState = new StateFunc(async () =>
            {
                if (Ref.Context != null)
                {
                    await JsRuntime.MoveEleTo(Ref, Container);
                }
            });
            _disableBodyScrollState = new StateFunc2(async () =>
            {
                await JsRuntime.EnableBodyScroll();
            }, async () =>
            {
                await JsRuntime.DisableBodyScroll();
            });

        }

        if (Visible)
        {
            if (_afterCallbackState is { State: true })
            {
                await _afterCallbackState.Invoke();
            }

            if (!string.IsNullOrWhiteSpace(Container))
            {
                if (!_moveEleToState!.State)
                {
                    if (Ref.Context != null)
                    {
                        await _moveEleToState.Invoke();
                    }
                }
            }

            if (!_disableBodyScrollState!.State)
            {
                await _disableBodyScrollState.Invoke();
            }
        }
        else if (!firstRender)
        {
            if (_disableBodyScrollState!.State)
            {
                await _disableBodyScrollState.Invoke();
            }

            if (DestroyOnClose && !_hasDestroyed)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(AniOptions?.Duration ?? AniConstant.MaxDuration));
                _hasDestroyed = true;
                _moveEleToState!.State = false;
                StateHasChanged();
            }

            if (_afterCallbackState is { State: false })
            {
                await _afterCallbackState.Invoke();
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }


    protected async Task HandleOnMaskClick()
    {
        if (OnMaskClick.HasDelegate)
        {
            await OnMaskClick.InvokeAsync();
        }

        if (CloseOnMaskClick)
        {
            await OnClose.InvokeAsync();
        }
    }

    protected async Task HandleCloserClick(MouseEventArgs e)
    {
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }
}


