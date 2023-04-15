namespace BlazorMix;

public abstract class BmComponentBase : ComponentBase, IDisposable
{
    protected void InvokeStateHasChanged()
    {
        InvokeAsync(() =>
        {
            if (!IsDisposed)
            {
                StateHasChanged();
            }
        });
    }

    protected async Task InvokeStateHasChangedAsync()
    {
        await InvokeAsync(() =>
        {
            if (!IsDisposed)
            {
                StateHasChanged();
            }
        });
    }


    #region IDisposable

    protected bool IsDisposed { get; private set; }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }
        IsDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
