using System.ComponentModel;

namespace BlazorMix;

public static class CancelEventArgsExt
{
    public static void Cancel(this CancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
    }
}