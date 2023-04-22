
using System.Security.Claims;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public class TransitionOptions
{
    internal TransitionState State { get; private set; } = TransitionState.Unmounted;

    public TransitionOptions()
    {
        bool exited, running;
        CalcClassBuilder(out exited, out running);
        CalcStyleBuilder(exited, running);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    internal void SetState(TransitionState state)
    {
        this.State = state;
        bool exited, running;
        CalcClassBuilder(out exited, out running);
        CalcStyleBuilder(exited, running);
    }

    public ClassBuilder Class { get; init; } = new ClassBuilder();
    public StyleBuilder Style { get; init; } = new StyleBuilder();


    private void CalcClassBuilder(out bool exited, out bool running)
    {
        var enter = TransitionState.Enter == State;
        var entering = TransitionState.Entering == State;
        var leave = TransitionState.Leave == State;
        var leaving = TransitionState.Leaving == State;
        exited = TransitionState.Leaved == State;
        running = enter || entering || leave || leaving;

        Class.Clear()
            .AddIf($"{TranisitionName}-enter", () => enter || entering)
            .AddIf($"{TranisitionName}-enter-active", () => entering)
            .AddIf($"{TranisitionName}-leave", () => leave || leaving)
            .AddIf($"{TranisitionName}-leave-active", () => leaving);
                ;
    }

    private void CalcStyleBuilder(bool exited, bool running)
    {
        var unmounted = TransitionState.Unmounted == State;

        var display = (unmounted || exited) && !Visible && !DestroyOnExit
            ? "none" : "";
        Style.Clear()
            .Add($"--za-animation-duration: {Duration}ms")
            .AddIf($"display: {display}", () => !string.IsNullOrWhiteSpace(display))
            ;
    }

    /// <summary>
    /// Tranisition Name
    /// </summary>
    internal string TranisitionName { get; set; } = "";

    /// <summary>
    /// The duration of the transition, in milliseconds.
    /// </summary>
    internal double Duration { get; set; } = 250;

    /// <summary>
    /// Triggers the enter or exit states
    /// </summary>
    internal bool Visible { get; set; }

    /// <summary>
    /// Destroy after transition finished
    /// </summary>
    internal bool DestroyOnExit { get; set; } = false;
}
