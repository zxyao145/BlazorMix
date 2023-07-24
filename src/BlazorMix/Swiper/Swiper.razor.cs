using BlazorMix.Core;
using Microsoft.VisualBasic;
using System.Drawing;

namespace BlazorMix;

public partial class Swiper
{
    public const string ClsPrefix = "bm-swiper";
    public const string ClsPrefixTrackInner = ClsPrefix + "-track-inner";

    #region Parameter

    /// <summary>
    /// 初始位置	
    /// </summary>
    [Parameter]
    public int DefaultIndex { get; set; } = 0;

    /// <summary>
    /// 是否允许手势滑动	
    /// </summary>
    [Parameter]
    public bool AllowTouchMove { get; set; } = true;

    /// <summary>
    /// 是否自动切换	
    /// </summary>
    [Parameter]
    public bool Autoplay { get; set; }

    /// <summary>
    /// 自动切换的间隔，单位为 ms	
    /// </summary>
    [Parameter] public int AutoplayInterval { get; set; } = 3000;

    /// <summary>
    /// 是否循环	
    /// </summary>
    [Parameter]
    public bool Loop { get; set; }

    /// <summary>
    /// 方向，默认是水平方向	
    /// </summary>
    [Parameter]
    public BmDirection Direction { get; set; } = BmDirection.Horizontal;

    /// <summary>
    /// 切换时触发	
    /// </summary>
    [Parameter]
    public EventCallback<int> OnIndexChange { get; set; }

    /// <summary>
    /// 指示器的相关属性	
    /// </summary>
    [Parameter]
    public PageIndicatorProps IndicatorProps { get; set; } = new ();

    /// <summary>
    /// 自定义指示器	
    /// </summary>
    [Parameter]
    public RenderFragment<IndicatorInfo>? Indicator { get; set; }

    /// <summary>
    /// 无指示器	
    /// </summary>
    [Parameter]
    public bool NoIndicator { get; set; } = false;

    /// <summary>
    /// 滑块的宽度百分比	
    /// </summary>
    [Parameter]
    public int SlideSize { get; set; } = 100;

    /// <summary>
    /// 滑块轨道整体的偏移量百分比	
    /// </summary>
    [Parameter]
    public int TrackOffset { get; set; }

    /// <summary>
    /// 是否在边界两边卡住，避免出现空白，仅在非 loop 模式且 slideSize < 100 时生效
    /// </summary>
    [Parameter]
    public bool StuckAtBoundary { get; set; } = true;

    /// <summary>
    /// 是否在拖动超出内容区域时启用橡皮筋效果，仅在非 loop 模式下生效
    /// </summary>
    [Parameter]
    public bool Rubberband { get; set; } = true;


    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion

    private bool _isVertical;
    private double _slideRatio;
    private double _offsetRatio;

    private readonly ClassBuilder _trackClassBuilder = new();
    private readonly StyleBuilder _trackInnerStyleBuilder = new();
    protected override void OnInitialized()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Direction.GetDisplayName()}")
            .Add(() => Class?.ToString() ?? "")
            ;
        _styleBuilder.Clear()
            .Add(() => Style?.ToString() ?? "")
            .Add(() => $"--slide-size:{SlideSize}%")
            .Add(() => $"--track-offset:{TrackOffset}%")
            ;
        _trackClassBuilder.Clear()
            .Add($"{ClsPrefix}-track")
            .AddIf($"{ClsPrefix}-track-allow-touch-move", () => AllowTouchMove)
            ;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _isVertical = Direction == BmDirection.Vertical;
        _slideRatio = SlideSize / 100.0;
        _offsetRatio = TrackOffset / 100.0;

        if (_timer != null)
        {
            _timer.Change(AutoplayInterval, AutoplayInterval);
        }
    }


    private ElementReference _rootElement;
    DomRect? _rect;
    private Timer? _timer;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _rect = await JsRuntime.GetBoundingClientRect(_rootElement);

            Current = DefaultIndex;

            _timer = new Timer((s) =>
            {
                if (_isInTouching)
                {
                    return;
                }

                Current += 1;
            }, null,
                 Autoplay ? AutoplayInterval : -1,
                AutoplayInterval
                );
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private int _current;

    internal int Current
    {
        get => _current;
        set
        {
            if (value > _items.Count - 1)
            {
                if (Loop)
                {
                    _current = 0;
                }
                else
                {
                    _current = _items.Count - 1;
                }
            }
            else if (value < 0)
            {
                if (Loop)
                {
                    _current = _items.Count - 1;
                }
                else
                {
                    _current = 0;
                }
            }
            else
            {
                _current = value;
            }

            UpdateTransform(_current);
        }
    }


    public void SwipePrev()
    {
        if (_current > 0)
        {
            Current = _current - 1;
        }
        else
        {
            Current = _items.Count - 1;
        }
    }


    public void SwipeNext()
    {
        if (_current < _items.Count - 1)
        {
            Current = _current + 1;
        }
        else
        {
            Current = 0;
        }
    }


    private List<SwiperItem> _items = new();

    internal void AddSwiperItem(SwiperItem item)
    {
        _items.Add(item);
        InvokeStateHasChanged();
    }

    #region Touch event

    private double _touchStartX, _touchStartY;
    private bool _isInTouching;

    // 百分比
    private double _maxPositionIndex = 0;
    private double _slidePixels = 0;
    private double _startOffset = 0;
    private Task OnTrackInnerTouchStart(TouchEventArgs arg)
    {
        if (AllowTouchMove && !_isInTouching)
        {
            _isInTouching = true;
            _touchStartX = arg.Touches[0].ClientX;
            _touchStartY = arg.Touches[0].ClientY;
            _maxPositionIndex = _items.Count - 1;
            _startOffset = _curOffset;
            _slidePixels = GetSlidePixels();
        }

        return Task.CompletedTask;
    }

    private Task OnTrackInnerTouchMove(TouchEventArgs arg)
    {
        if (_isInTouching)
        {
            if (_slidePixels < 0.001)
            {
                return Task.CompletedTask;
            }

            var point = arg.Touches[0];

            var relativeOffset = _isVertical ? (point.ClientY - _touchStartY) : (point.ClientX - _touchStartX);
            var positionIndex = (_startOffset - relativeOffset) / _slidePixels;

            if (positionIndex < -0.1)
            {
                positionIndex = -0.1;
            }
            else if (positionIndex > _maxPositionIndex + 0.1)
            {
                positionIndex = _maxPositionIndex + 0.1;
            }

            UpdateTransform(positionIndex);
        }

        return Task.CompletedTask;
    }

    private Task OnTrackInnerTouchEnd(TouchEventArgs arg)
    {
        if (AllowTouchMove && _isInTouching)
        {
            var point = arg.ChangedTouches[0];

            var relativeOffset = _isVertical ? (point.ClientY - _touchStartY) : (point.ClientX - _touchStartX);
            var real = - relativeOffset / _slidePixels;
            Console.WriteLine("relativeOffset:{0},_slidePixels:{1}", relativeOffset, _slidePixels);

            //var minIndex = (int)Math.Floor(real);
            //var maxIndex = minIndex + 1;
            //var current = (real) % 1 > 0.5 ? maxIndex : minIndex;

            var current = (int)Math.Round(real);
            Console.WriteLine("real:{0},current:{1},{2}", real, current, Current);

            Current = Current + current;

            _isInTouching = false;
        }

        return Task.CompletedTask;
    }

    #endregion

    private double _curOffset;

    private void UpdateTransform(double position)
    {
        position = BoundIndex(position);
        if (_isVertical)
        {
            var delta = _rect!.Height;
            _curOffset = position * delta;
        }
        else
        {
            var delta = _rect!.Width;
            _curOffset = position * delta;
        }

        _trackInnerStyleBuilder.Clear()
            .Add(_isVertical
                ? $"transform: translate3d(0px, -{position * 100}%, 0px)"
                : $"transform: translate3d(-{position * 100}%, 0px, 0px)");
        InvokeStateHasChanged();
    }

    private double BoundIndex(double position)
    {
        var min = 0.0;
        var max = _items.Count - 1.0;
        if (StuckAtBoundary)
        {
            min += _offsetRatio / _slideRatio;
            max -= _offsetRatio / _slideRatio;
        }
        var res = Math.Max(position, min);
        res = Math.Min(res, max);
        return res;
    }

    private double GetSlidePixels()
    {
        if (_rect == null)
        {
            return 0;
        }
        var trackPixels = _isVertical ? _rect.Height : _rect.Width;
        return trackPixels;
    }

    protected override void Dispose(bool disposing)
    {
        _timer?.Dispose();
        base.Dispose(disposing);
    }
}
