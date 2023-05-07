namespace BlazorMix;

public class ActionSheetRef : IRef
{
    public ActionSheetRef(ActionSheetService service, ActionSheetOption option)
    {
        this.Option = option;
        this.Service = service;
    }

    internal ActionSheetService? Service { get; set; }

    internal ActionSheetOption Option { get; }

    public async ValueTask Close()
    {
        if (Service != null)
        {
            await Service.Close(this.Option);
        }
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is ActionSheetRef eleRef)
        {
            return Equals(eleRef);
        }

        return false;
    }

    protected bool Equals(ActionSheetRef other)
    {
        return Option.Equals(other.Option);
    }

    public override int GetHashCode()
    {
        return Option.GetHashCode();
    }
}
