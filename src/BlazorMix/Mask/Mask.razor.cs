
using BlazorMix.Core;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Mask 
{
    private ElementReference _maskRef;

    private StateFunc? _moveEleToState;
    private StateFunc2? _disableBodyScrollState;
    private StateFunc2? _afterCallbackState;
    private bool _hasDestroyed = false;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var lastVisible = Visible;
        await base.SetParametersAsync(parameters);


        _classBuilder.Clear()
            .Add(PrefixCls);

        var rgb = Color == "black" ? "0, 0, 0" : "255, 255, 255";
        var bgColor = Color == "transparent"
            ? "transparent"
            : $"rgba({rgb}, {Opacity})";
        _styleBuilder.Clear()
            .Add($"background-color: {bgColor}")
            .AddIf("display: none", () => AniOptions == null && !Visible);

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
                    if (_maskRef.Context != null)
                    {
                        await JsRuntime.MoveEleTo(_maskRef, Container);
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

        await base.OnAfterRenderAsync(firstRender);
        if (Visible)
        {
            if (_hasDestroyed)
            {
                _hasDestroyed = false;
                StateHasChanged();
            }

            if (!string.IsNullOrWhiteSpace(Container))
            {
                if (!_moveEleToState!.State)
                {
                    await _moveEleToState.Invoke();
                }
            }

            if (DisableBodyScroll)
            {
                if (!_disableBodyScrollState!.State)
                {
                    await _disableBodyScrollState.Invoke();
                }
            }

            if (_afterCallbackState is { State: true })
            {
                await _afterCallbackState.Invoke();
            }
        }
        else if(!firstRender)
        {
            if (_disableBodyScrollState!.State)
            {
                await _disableBodyScrollState.Invoke();
            }

            if (DestroyOnClose && !_hasDestroyed)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(AniOptions?.Duration ?? 0));
                _hasDestroyed = true;
                _moveEleToState!.State =false;
                StateHasChanged();
            }

            if (_afterCallbackState is { State: false })
            {
                await _afterCallbackState.Invoke();
            }
        }
    }

    private async Task HandleMaskClick(MouseEventArgs eventArgs)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
