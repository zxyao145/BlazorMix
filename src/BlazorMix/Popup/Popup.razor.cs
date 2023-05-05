namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Popup
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-popup";

    private Position _pos = Position.Bottom;

    /// <summary>
    /// 指定弹出的位置	
    /// </summary>
    [Parameter]
    public Position Position
    {
        get => _pos;
        set
        {
            if (_pos == value)
            {
                return;
            }

            _pos = value;
            _animateName = _pos switch
            {
                Position.Top => AniNames.FadeDown,
                Position.Left => AniNames.FadeLeft,
                Position.Right => AniNames.FadeRight,
                _ => AniNames.FadeUp
            };
        }
    }

    private string _animateName = AniNames.FadeUp;

    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        _classBuilder
            .Add($"{PrefixCls}-body")
            .Add(() => Class?.ToString() ?? "")
            .Add($"{PrefixCls}-body-position-{Position.GetDisplayName()}");
    }
}
