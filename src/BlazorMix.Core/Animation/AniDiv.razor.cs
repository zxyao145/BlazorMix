
// ReSharper disable CheckNamespace

using Microsoft.AspNetCore.Components.Web;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class AniDiv 
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private readonly AniOptions _options = new AniOptions();

    #region paramters

    /// <inheritdoc cref="AniOptions.Name"/>
    [Parameter]
    public string Name { get => _options.Name; set => _options.Name = value; }

    /// <inheritdoc cref="AniOptions.In"/>
    [Parameter]
    public bool In { get => _options.In; set => _options.In = value; }

    /// <inheritdoc cref="AniOptions.Duration"/>
    [Parameter]
    public double Duration { get => _options.Duration; set => _options.Duration = value; }

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

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    private readonly Queue<Func<ValueTask>> _actionQueue = new Queue<Func<ValueTask>>();

    private DateTime _beginTime;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var lastState = In;
        await base.SetParametersAsync(parameters);
        if (lastState != In && In)
        {
            _beginTime = DateTime.UtcNow;
            OnEnter?.Invoke();
            _options.SetState(AniState.Enter);
            OnEntering?.Invoke();
        }
        else if (lastState != In)
        {
            _beginTime = DateTime.UtcNow;
            OnExit?.Invoke();
            _options.SetState(AniState.Leave);
            OnExiting?.Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Console.WriteLine("Animation _options.State:{0}", _options.State);
        if (In)
        {
            switch (_options.State)
            {
                case AniState.Leave:
                    _actionQueue.Enqueue(() =>
                    {
                        _beginTime = DateTime.UtcNow;
                        OnEnter?.Invoke();
                        _options.SetState(AniState.Enter);
                        OnEntering?.Invoke();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case AniState.Enter:
                    _actionQueue.Enqueue(() =>
                    {
                        _options.SetState(AniState.Entering);
                        OnEnterEnd?.Invoke();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case AniState.Entering:
                    _actionQueue.Enqueue(async () =>
                    {
                        await Wait();
                        _options.SetState(AniState.Entered);
                        OnEnterEnd?.Invoke();
                    });
                    break;
            }
        }
        else
        {
            switch (_options.State)
            {
                case AniState.Leave:
                    _actionQueue.Enqueue(() =>
                    {
                        _options.SetState(AniState.Leaving);
                        OnExitEnd?.Invoke();
                        return ValueTask.CompletedTask;
                    });
                    break;
                case AniState.Leaving:
                    _actionQueue.Enqueue(async () =>
                    {
                        await Wait();
                        _options.SetState(AniState.Leaved);
                    });
                    break;
            }
        }


        if (_actionQueue.Count > 0)
        {
            while (_actionQueue.Count > 0)
            {
                var func = _actionQueue.Dequeue();
                await func();
            }
            StateHasChanged();
        }
    }


    private async ValueTask Wait()
    {
        var d = Duration - (DateTime.UtcNow - _beginTime).TotalMilliseconds;
        if(d > 0)
        {
            await Task.Delay((int)d);
        }
    }


    private async Task HandleClick(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (this.In )
        {
            this.In = false;
        }
        base.Dispose(disposing);
    }
}
