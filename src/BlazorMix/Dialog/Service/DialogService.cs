
namespace BlazorMix;
public class DialogService
{
    internal event Func<DialogRef, Task>? OnAddEvent;
    internal event Func<Task>? OnUpdateEvent;
    internal event Func<Task>? OnClear;


    public Task Alert(DialogAlertOption option)
    {
        option.Visible = true;
        option.CloseOnAction = true;
        _ = AddOrUpdate(option);
        return option.Tsc.Task;
    }

    public Task<bool> Confirm(DialogConfirmOption option)
    {
        option.Visible = true;
        option.CloseOnAction = true;
        _ = AddOrUpdate(option);
        return option.Tsc.Task;
    }

    public async Task<DialogRef?> Show(DialogOption option)
    {
        option.Visible = true;
        return await AddOrUpdate(option);
    }

    private async Task<DialogRef?> AddOrUpdate(DialogOption option)
    {
        if (OnAddEvent == null)
        {
            return null;
        }

        var r = new DialogRef(this, option);
        if (!option.Added)
        {
            await OnAddEvent.Invoke(r);
        }
        else if (OnUpdateEvent != null)
        {
            await OnUpdateEvent.Invoke();
        }
        return r;
    }


    public async Task Close(DialogRef dialogRef)
    {
        if (OnUpdateEvent != null)
        {
            dialogRef.Option.Visible = false;
            await OnUpdateEvent.Invoke();
        }
    }

    public async Task Clear()
    {
        if (OnClear != null)
        {
            await OnClear.Invoke();
        }
    }
}
