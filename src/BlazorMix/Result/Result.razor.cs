namespace BlazorMix;

public partial class Result
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-result";

    [Parameter] public ResultStatus Status { get; set; }

    [Parameter, EditorRequired] public StringOrRenderFragment Title { get; set; }

    [Parameter] public RenderFragment? Description { get; set; }

    [Parameter] public RenderFragment? Icon { get; set; } = null;


    protected override Task OnInitializedAsync()
    {
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add($"{ClsPrefix}-{Status.GetDisplayName()}")
            .Add(() => Class?.ToString() ?? "");
        return base.OnInitializedAsync();
    }

    private static readonly Dictionary<ResultStatus, RenderFragment> DefaultIcon = new()
    {
        { ResultStatus.Success, b => b.Fluent().OpenComponent<CheckCircleFill>().CloseComponent() },
        { ResultStatus.Error, b => b.Fluent().OpenComponent<CloseCircleFill>().CloseComponent() },
        { ResultStatus.Info, b => b.Fluent().OpenComponent<InformationCircleFill>().CloseComponent() },
        { ResultStatus.Waiting, b => b.Fluent().OpenComponent<ClockCircleFill>().CloseComponent() },
        { ResultStatus.Warning, b => b.Fluent().OpenComponent<ExclamationCircleFill>().CloseComponent() },
    };

    private RenderFragment? _resultIcon = null;
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _resultIcon = Icon ?? DefaultIcon[Status];
    }
}