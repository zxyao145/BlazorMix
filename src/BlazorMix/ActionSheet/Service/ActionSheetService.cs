namespace BlazorMix;
public class ActionSheetService
{
    internal event Func<ActionSheetRef, ValueTask>? OnAddEvent;

    internal event Func<ValueTask>? OnUpdateEvent;

    public async Task<ActionSheetRef?> Add(ActionSheetOption option)
    {
        if (OnAddEvent == null)
        {
            return null;
        }
        var r = new ActionSheetRef(this, option);

        if (!option.Added)
        {
            await OnAddEvent.Invoke(r);
        }
        return r;
    }

    public async Task<ActionSheetRef?> Show(ActionSheetOption option)
    {
        if (OnAddEvent == null)
        {
            return null;
        }

        option.Visible = true;
        if (!option.Added)
        {
            return await Add(option);
        }

        if (OnUpdateEvent != null)
        {
            await OnUpdateEvent.Invoke();
        }
        return new ActionSheetRef(this, option);
    }

    public async Task Close(ActionSheetOption option)
    {
        if (OnUpdateEvent != null)
        {
            option.Visible = false;
            await OnUpdateEvent.Invoke();
        }
    }
}
