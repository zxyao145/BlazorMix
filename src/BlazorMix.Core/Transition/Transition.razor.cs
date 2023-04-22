
namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Transition : BmDomComponentBase
{

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private TransitionOptions _options = new TransitionOptions();

    #region paramters

    /// <inheritdoc cref="TransitionOptions.Name"/>
    [Parameter]
    public string Name { get => _options.TranisitionName; set => _options.TranisitionName = value; }

    /// <inheritdoc cref="TransitionOptions.Visible"/>
    [Parameter]
    public bool Visible { get => _options.Visible; set => _options.Visible = value; }

    /// <inheritdoc cref="TransitionOptions.Duration"/>
    [Parameter]
    public double Duration { get => _options.Duration; set => _options.Duration = value; }

    /// <inheritdoc cref="TransitionOptions.DestroyOnExit"/>
    [Parameter]
    public bool DestroyOnExit { get => _options.DestroyOnExit; set => _options.DestroyOnExit = value; }


    #region event

    /// <summary>
    /// Callback fired before the "entering" status is applied.
    /// </summary>
    [Parameter]
    public Action? OnEnter { get; set; }

    /// <summary>
    /// Callback fired after the "entering" status is applied. 
    /// </summary>
    [Parameter]
    public Action? OnEntering { get; set; }

    /// <summary>
    /// Callback fired after the "entered" status is applied.
    /// </summary>
    [Parameter]
    public Action? OnEnterEnd { get; set; }

    /// <summary>
    /// Callback fired before the "exiting" status is applied.
    /// </summary>
    [Parameter]
    public Action? OnExit { get; set; }

    /// <summary>
    /// Callback fired after the "exiting" status is applied.
    /// </summary>
    [Parameter]
    public Action? OnExiting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Action? OnExitEnd { get; set; } 
    #endregion

    #endregion


    private Queue<Func<ValueTask>> _actionQueue = new Queue<Func<ValueTask>>();

    private DateTime beginTime;
    private bool _hasDestroyed = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (Visible && !_hasDestroyed)
        {
            beginTime = DateTime.UtcNow;
            OnEnter?.Invoke();
            _options.SetState(TransitionState.Enter);
            OnEntering?.Invoke();
        }
        else if (_options.State == TransitionState.Entered)
        {
            beginTime = DateTime.UtcNow;
            OnExit?.Invoke();
            _options.SetState(TransitionState.Leave);
            OnExiting?.Invoke();
        }
    }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        Console.WriteLine($"OnAfterRenderAsync:{Visible},{_options.State},{DestroyOnExit}");
        if (Visible)
        {
            switch (_options.State)
            {
                case TransitionState.Leave:
                    _actionQueue.Enqueue(() =>
                    {
                        beginTime = DateTime.UtcNow;
                        OnEnter?.Invoke();
                        _options.SetState(TransitionState.Enter);
                        OnEntering?.Invoke();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case TransitionState.Enter:
                    _actionQueue.Enqueue(() =>
                    {
                        _options.SetState(TransitionState.Entering);
                        OnEnterEnd?.Invoke();
                        StateHasChanged();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case TransitionState.Entering:
                    _actionQueue.Enqueue(async () =>
                    {
                        await Wait();;
                        _options.SetState(TransitionState.Entered);
                        OnEnterEnd?.Invoke();
                        StateHasChanged();
                    });
                    break;
            }
        }
        else
        {
            switch (_options.State)
            {
                case TransitionState.Leave:
                    _actionQueue.Enqueue(() =>
                    {
                        _options.SetState(TransitionState.Leaving);
                        OnExitEnd?.Invoke();
                        StateHasChanged();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case TransitionState.Leaving:
                    _actionQueue.Enqueue(async () =>
                    {
                        await Wait();
                        if (DestroyOnExit && !_hasDestroyed)
                        {
                            _hasDestroyed = true;
                            _options.SetState(TransitionState.Unmounted);
                        }
                        else
                        {
                            _options.SetState(TransitionState.Leaved);
                        }
                        StateHasChanged();
                    });
                    break;
            }
        }
        


        while (_actionQueue.Count > 0)
        {
            var func = _actionQueue.Dequeue();
            await func();
        }
    }


    private async ValueTask Wait()
    {
        var d = Duration - (DateTime.UtcNow - beginTime).TotalMilliseconds;
        Console.WriteLine(d);
        if(d > 0)
        {
            await Task.Delay((int)d);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (this.Visible && !this._hasDestroyed)
        {
            this.Visible = false;
            this._hasDestroyed = true;
        }
        base.Dispose(disposing);
    }
}
