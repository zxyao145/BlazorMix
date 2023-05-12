
namespace BlazorMix;
public class DialogAlertOption : DialogOption
{
    private readonly DialogActionItem _dialogActionItem;
    public DialogAlertOption()
    {
        _dialogActionItem = new DialogActionItem()
        {
            Key = "ok",
            Text = "我知道了",
            OnClick = HandleConfirmClick
        };
        Actions = new()
        {
            _dialogActionItem
        }; 
    }


    public StringOrRenderFragment ConfirmText
    {
        get => _dialogActionItem.Text;
        set => _dialogActionItem.Text = value;
    }

    public Func<DialogActionEventArgs, Task>? OnConfirm
    {
        get;
        set;
    }

    internal TaskCompletionSource Tsc = new();

    private async Task HandleConfirmClick(DialogActionEventArgs e)
    {
        if (OnConfirm != null)
        {
            await OnConfirm.Invoke(e);
        }

        Tsc.TrySetResult();
    }
}
