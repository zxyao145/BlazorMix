using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMix.Core;
internal class JsConstants
{
    public const string Prefix = "Bm.";


    #region common


    public const string PrefixCommon = "Bm.common.";

    public const string MoveEleTo = $"{PrefixCommon}{nameof(MoveEleTo)}";
    public const string DisableBodyScroll = $"{PrefixCommon}{nameof(DisableBodyScroll)}";
    public const string EnableBodyScroll = $"{PrefixCommon}{nameof(EnableBodyScroll)}";

    #endregion

}
