using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.CompilerServices;

namespace BlazorMix;
internal partial class Toast : BmDomComponentBase
{
    public const string ClsPrefix = "bm-toast";

    private readonly ToastOption _props;

    private readonly StyleBuilder _top;
    private readonly ClassBuilder _mainClass;
    private RenderFragment? _iconElement = null;

    public Toast(ToastOption props)
    {
        _props = props;

        _mainClass = new ClassBuilder();
        _mainClass.Add($"{ClsPrefix}-main")
            .Add(() => _props.Icon != null
                ? $"{ClsPrefix}-main-icon"
                : $"{ClsPrefix}-main-text"
            );
        _top = new StyleBuilder();
        _top.Add(() => _props.Position == ToastPosition.Top
                ? "top: 20%"
                : _props.Position == ToastPosition.Bottom
                    ? "top: 80%"
                    : "top: 50%"
            );
    }


    public RenderFragment Node => BuildRenderTree;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (_props.Icon != null)
        {
            if (_props.Icon.Value.IsT0)
            {
                _iconElement = _props.Icon.Value.AsT0 switch
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
                _iconElement = _props.Icon.Value.AsT1;
            }
        }

        var fluentRenderTreeBuilder = builder.Fluent();
        fluentRenderTreeBuilder
            .OpenComponent<AniMask>()
            .AddAttribute("Visible", _props.Visible)
            .AddAttribute("DestroyOnClose", true)
            .AddAttribute("Opacity", RuntimeHelpers.TypeCheck<Double>(0))
            .AddAttribute("DisableBodyScroll", false)
            .AddAttribute("Container", "")
            .AddAttribute("AfterClose", _props.AfterClose)
            .AddAttribute("Style", 
                RuntimeHelpers.TypeCheck<StyleBuilder>(
                $";pointer-events:{(_props.MaskClickable ? "none" : "auto")};{_props.MaskStyle}"
                ))
            .AddAttribute("Class", RuntimeHelpers.TypeCheck<ClassBuilder>(
                $" {ClsPrefix}-mask {_props.MaskClass}"
                ))
            .AddAttribute("ChildContent", (RenderFragment)((builder2) =>
            {
                var fb2 = builder2.Fluent(fluentRenderTreeBuilder.CurSequence);
                fb2.OpenElement("div")
                    .AddAttribute("class", $"{ClsPrefix}-wrap")
                    .OpenElement("div")
                    .AddAttribute("style", _top.ToString())
                    .AddAttribute("class", _mainClass.ToString());
                if (_iconElement != null)
                {
                    fb2.OpenElement("div")
                        .AddAttribute("class", $"{ClsPrefix}-icon")
                        .AddContent(_iconElement)
                        .CloseElement();
                }
                fb2.OpenComponent<AutoCenter>()
                    .AddAttribute("ChildContent", (RenderFragment)((builder3) =>
                        {
                            builder3.AddContent(fb2.CurSequence + 1, _props.Content.Node);
                        }
                    ))
                    .CloseComponent()
                    .CloseElement()
                    .CloseElement();
            }
        ));
        fluentRenderTreeBuilder
            .CloseComponent();
    }

}
