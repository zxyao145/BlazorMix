
namespace BlazorMix;


public partial class Space
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-space";

    #region Parameter

    /// <summary>
    /// start | end |center |baseline
    /// </summary>
    [Parameter]
    public string Align { get; set; } = "";

    /// <summary>
    /// Cross axis alignment
    /// </summary>
    [Parameter]
    public string Justify { get; set; } = "";

    /// <summary>
    /// Whether to render as block level elements
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Whether to automatically wrap, only valid when horizontal.
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; }

    /// <summary>
    /// Spacing direction
    /// </summary>
    [Parameter]
    public BmDirection Direction { get; set; } = BmDirection.Horizontal;

    /// <summary>
    /// Click event of Space.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Direction.GetDisplayName()}")
            .AddIf($"{ClsPrefix}-block", () => Block)
            .AddIf($"{ClsPrefix}-wrap", () => Wrap)
            .AddIf($"{ClsPrefix}-justify-{Justify}", () => !string.IsNullOrWhiteSpace(Justify))
            .AddIf($"{ClsPrefix}-align-{Align}", () => !string.IsNullOrWhiteSpace(Align))
            ;
    }
}
