using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix;
public class DialogRef: IRef
{
    public DialogRef(DialogService service, DialogOption option)
    {
        this.Option = option;
        this.Service = service;
    }

    internal DialogService? Service { get; set; }

    internal DialogOption Option { get; }

    public async ValueTask Close()
    {
        if (Service != null)
        {
            await Service.Close(this);
        }
    }
}
