namespace BlazorMix;
public partial class ListItem
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-list-item";

    [CascadingParameter, EditorRequired]
    public List Parent { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? Description { get; set; }

    [Parameter]
    public RenderFragment? Prefix { get; set; }

    [Parameter]
    public RenderFragment? Title { get; set; }

    [Parameter]
    public RenderFragment? Extra { get; set; }

    [Parameter]
    public bool Clickable { get; set; }

    [Parameter]
    public OneOf<bool, RenderFragment> Arrow { get; set; } = false;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }

    internal bool _clickable;
    internal OneOf<bool, RenderFragment> _arrow;

    protected override async Task OnInitializedAsync()
    {
        _classBuilder
            .Add(ClsPrefix)
            .AddIf("plain-anchor", ()=> Clickable || OnClick.HasDelegate)
            .AddIf($"{ClsPrefix}-disabled", ()=> Disabled)
            .Add(() => Class?.ToString() ?? "");
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _clickable = Clickable || OnClick.HasDelegate;
        _arrow = Arrow is { IsT0: true, AsT0: false } ? _clickable : Arrow;
    }

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
