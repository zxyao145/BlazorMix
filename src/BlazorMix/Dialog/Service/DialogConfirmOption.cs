namespace BlazorMix;
public class DialogConfirmOption : DialogOption
{
    private readonly DialogActionItem _dialogCancelActionItem;
    private readonly DialogActionItem _dialogOkActionItem;

    public DialogConfirmOption()
    {
        _dialogCancelActionItem = new DialogActionItem()
        {
            Key = "cancel",
            Text = "取消",
            OnClick = HandleCancelClick
        };
        _dialogOkActionItem = new DialogActionItem()
        {
            Key = "ok",
            Text = "确定",
            OnClick = HandleConfirmClick
        };
        Actions = new()
        {
            new List<DialogActionItem>()
            {
                _dialogCancelActionItem,
                _dialogOkActionItem
            }
        }; 
    }


    public StringOrRenderFragment CancelText
    {
        get => _dialogCancelActionItem.Text;
        set => _dialogCancelActionItem.Text = value;
    }

    public StringOrRenderFragment ConfirmText
    {
        get => _dialogOkActionItem.Text;
        set => _dialogOkActionItem.Text = value;
    }

    internal TaskCompletionSource<bool> Tsc = new();

    private async Task HandleConfirmClick(DialogActionEventArgs e)
    {
        if (OnConfirm == null)
        {
            Tsc.TrySetResult(true);
        }
        else
        {
            try
            {
                await OnConfirm.Invoke(e);
                Tsc.TrySetResult(true);
            }
            catch (Exception ex)
            {
                e.Cancel();
                Console.WriteLine(ex);
            }
        }
    }

    private async Task HandleCancelClick(DialogActionEventArgs e)
    {
        if (OnCancel != null)
        {
            await OnCancel.Invoke(e);
        }
        Tsc.TrySetResult(false);
    }

    public Func<DialogActionEventArgs, Task>? OnCancel
    {
        get; set;
    }
    
    public Func<DialogActionEventArgs, Task>? OnConfirm
    {
        get; set;
    }
}
