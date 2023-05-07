namespace BlazorMix;
public class ToastRef : IRef
{
    public ToastRef(ToastService service, ToastOption option)
    {
        this.Option = option;
        this.Service = service;
    }

    internal ToastService? Service { get; set; }

    internal ToastOption Option { get; }

    public async ValueTask Close()
    {
        if(Service != null)
        {
            await Service.Close(this);
        }
    }

}
