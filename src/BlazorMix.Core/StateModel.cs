
using System.Runtime.CompilerServices;

namespace BlazorMix;

public class StateFunc
{
    private readonly Func<ValueTask> _func;

    public bool State { get; set; }

    public StateFunc(Func<ValueTask> func, bool defaultState = false)
    {
        this._func = func;
        this.State = defaultState;
    }

    public async ValueTask Invoke()
    {
        State = !State;
        await _func.Invoke();
    }
}

public class StateFunc2
{
    private readonly Func<ValueTask> _trueFunc;
    private readonly Func<ValueTask> _falseFunc;

    public bool State { get; set; }

    public StateFunc2(Func<ValueTask> trueFunc, 
        Func<ValueTask> falseFunc, 
        [CallerLineNumber] int line = 0)
    {
        this._trueFunc = trueFunc;
        this._falseFunc = falseFunc;
    }

    public async ValueTask Invoke()
    {
        State = !State;
        if (State)
        {
            await _falseFunc.Invoke();
        }
        else
        {
            await _trueFunc.Invoke();
        }
    }
}
