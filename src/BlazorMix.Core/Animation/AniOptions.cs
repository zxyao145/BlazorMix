
using System.Security.Claims;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public class AniOptions
{
    public AniState State { get; private set; } = AniState.Unmounted;

    public AniOptions()
    {
        SetState(AniState.Unmounted);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    internal void SetState(AniState state)
    {
        this.State = state;
        CalcClassBuilder(out bool exited, out bool running);
        CalcStyleBuilder(exited, running);
    }

    public ClassBuilder Class { get; init; } = new ClassBuilder("anioptions");
    public StyleBuilder Style { get; init; } = new StyleBuilder("anioptions");


    private void CalcClassBuilder(out bool exited, out bool running)
    {
        var enter = AniState.Enter == State;
        var entering = AniState.Entering == State;
        var leave = AniState.Leave == State;
        var leaving = AniState.Leaving == State;
        exited = AniState.Leaved == State;
        running = enter || entering || leave || leaving;

        Class.Clear()
            .AddIf(()=> $"bm{Name}-enter", val => enter || entering)
            .AddIf(()=> $"bm{Name}-enter-active", val => entering)
            .AddIf(()=> $"bm{Name}-leave", val => leave || leaving)
            .AddIf(()=> $"bm{Name}-leave-active", val => leaving);
    }

    private void CalcStyleBuilder(bool exited, bool running)
    {
        var unmounted = AniState.Unmounted == State;

        string GetDisplay()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                return (unmounted || exited) && !In
                    ? "display: none;"
                    : "";
            }

            return !In ? "display: none;" : "";
        }

        Style.Clear()
            .Add($"--animation-duration: {Duration}ms;")
            .Add(()=> $"{GetDisplay()}")
            ;
    }

    /// <summary>
    /// Transition Name
    /// </summary>
    internal string Name { get; set; } = "";

    /// <summary>
    /// The duration of the transition, in milliseconds.
    /// </summary>
    public double Duration { get; internal set; } = 300;

    /// <summary>
    /// Triggers the enter or exit states
    /// </summary>
    public bool In { get; set; }
}
