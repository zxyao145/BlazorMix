namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Popup
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-popup";

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

    protected override void OnAfterRender(bool firstRender)
    {
        _classBuilder
            .Add($"{ClsPrefix}-body")
            .Add(() => BodyClass?.ToString() ?? "")
            .Add($"{ClsPrefix}-body-position-{Position.GetDisplayName()}");

        base.OnAfterRender(firstRender);
    }
}
