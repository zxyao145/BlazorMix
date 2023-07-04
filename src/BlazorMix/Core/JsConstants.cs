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
    public const string ObserveInViewportOnce = $"{PrefixCommon}{nameof(ObserveInViewportOnce)}";

    #endregion

    public const string PrefixNoticeBar = "Bm.NoticeBar.";
    public const string NoticeBarStartScroll = $"{PrefixNoticeBar}startScroll";
    public const string NoticeBarEndScroll = $"{PrefixNoticeBar}endScroll";


    public const string PrefixSwipeActionBar = "Bm.SwipeAction.";
    public const string SwipeActionInit = $"{PrefixSwipeActionBar}init";
    public const string SwipeActionClose = "close";    
    
    
    public const string PrefixEllipsis = "Bm.Ellipsis.";
    public const string EllipsisCalcEllipsised = $"{PrefixEllipsis}calcEllipsised";   
    
    
    public const string PrefixWaterMark = "Bm.WaterMark.";
    public const string PrefixWaterMarkRender = $"{PrefixWaterMark}render";


    public const string PrefixFloatingPanel = "Bm.FloatingPanel.";
    public const string FloatingPanelInit = $"{PrefixFloatingPanel}init";
}
