namespace BlazorMix;
public class ToastService
{
    internal event Func<ToastRef, ValueTask>? OnAddEvent;
    internal event Func<ValueTask>? OnRemove;
    internal event Func<ValueTask>? OnUpdateEvent;

    private Task? _lastClearTask;
    private CancellationTokenSource? _cts;

    /// <summary>
    /// 显示一个 Toast。注意：同一时间只允许显示一个 Toast，新出现的 Toast 会将之前正在显示中的 Toast 挤掉。
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public async ValueTask<ToastRef?> Show(ToastOption option)
    {
        if (OnAddEvent == null)
        {
            return null;
        }
        var r = new ToastRef(this, option);
        await OnAddEvent.Invoke(r);

        if (_lastClearTask is { IsCompleted: false } && _cts != null)
        {
            _cts.Cancel(true);
        }

        var duration = option.DurationMs;
        if (duration <= 0)
        {
            _cts = null;
            _lastClearTask = null;
            return r;
        }
        _cts = new CancellationTokenSource();
        _lastClearTask = Task.Run(async () =>
        {
            await Task.Delay(duration, _cts.Token);
            await Close(r);
        });

        return r;
    }

    /// <summary>
    /// 更新 options 后，调用次方法可以更新 Toast
    /// </summary>
    /// <returns></returns>
    public async ValueTask Update()
    {
        if (OnUpdateEvent != null)
        {
            await OnUpdateEvent.Invoke();
        }
    }

    /// <summary>
    /// 关闭所有显示中的 Toast。
    /// </summary>
    /// <returns></returns>
    public async ValueTask Clear()
    {
        if (OnRemove != null)
        {
            await OnRemove();
        }
    }


    /// <summary>
    /// 关闭所有显示中的 Toast。
    /// </summary>
    /// <param name="toastRef"></param>
    /// <returns></returns>
    public async ValueTask Close(ToastRef toastRef)
    {
        await Clear();
    }

    /// <summary>
    /// 全局配置，支持配置 Duration、Position 和 MaskClickable
    /// </summary>
    /// <param name="durationMs"></param>
    /// <param name="position"></param>
    /// <param name="maskClickable"></param>
    public void Config(
        int? durationMs = null,
        ToastPosition? position = null,
        bool? maskClickable = null
        )
    {
        if (durationMs != null)
        {
            ToastOption.GlobalDurationMs = durationMs.Value;
        }
        if (position != null)
        {
            ToastOption.GlobalPosition = position.Value;
        }
        if (maskClickable != null)
        {
            ToastOption.GlobalMaskClickable = maskClickable.Value;
        }
    }
}
