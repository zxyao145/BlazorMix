namespace BlazorMix;
public partial class Toast
{
    /// <summary>
    /// 
    /// </summary>
    public const string PrefixCls = "bm-toast";

    [Parameter]
    public ToastOption Props { get; set; }

    private StyleBuilder _top = new StyleBuilder();
    private ClassBuilder _mainClass = new ClassBuilder();
    private RenderFragment? _iconElement = null;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (Props.Icon != null)
        {
            if (Props.Icon.Value.IsT0)
            {
                _iconElement = Props.Icon.Value.AsT0 switch
                {
                    ToastIconType.Success => b => b.Fluent()
                    .OpenComponent<CheckOutline>()
                    .CloseComponent(),
                    ToastIconType.Fail => b => b.Fluent()
                        .OpenComponent<CloseOutline>()
                        .CloseComponent(),
                    _ => b => b.Fluent()
                        .OpenComponent<LoadingIcon>()
                        .CloseComponent()
                };
            }
            else
            {
                _iconElement = Props.Icon.Value.AsT1;
            }
        }

        _mainClass
            .Clear()
            .Add($"{PrefixCls}-main")
            .Add(() => Props.Icon != null ? $"{PrefixCls}-main-icon" : $"{PrefixCls}-main-text")
            .Add(() => Props.Position == ToastPosition.Top
                ? "top: 20%"
                : Props.Position == ToastPosition.Bottom
                    ? "top: 80%"
                    : "top: 50%"
            );
    }
}
