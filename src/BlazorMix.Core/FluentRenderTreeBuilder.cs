
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorMix;

public static class FluentRenderTreeBuilderExt
{
    public static FluentRenderTreeBuilder Fluent(this RenderTreeBuilder builder, int initSequence = 0)
    {
        return new FluentRenderTreeBuilder(builder, initSequence);
    }
}

public class FluentRenderTreeBuilder
{
    private readonly RenderTreeBuilder _builder;
    private int _sequence;

    public int CurSequence => _sequence;

    private int GetSequence()
    {
        return _sequence++;
    }

    public FluentRenderTreeBuilder(RenderTreeBuilder builder, int initSequence = 0)
    {
        _builder = builder;
        _sequence = initSequence;
    }

    public FluentRenderTreeBuilder OpenComponent<TComponent>() where TComponent : notnull, IComponent
    {
        _builder.OpenComponent<TComponent>(GetSequence());
        return this;
    }

    public FluentRenderTreeBuilder CloseComponent()
    {
        _builder.CloseComponent();
        return this;
    }


    public FluentRenderTreeBuilder OpenElement(string elementName)
    {
        _builder.OpenElement(GetSequence(), elementName);
        return this;
    }
    public FluentRenderTreeBuilder CloseElement()
    {
        _builder.CloseElement();
        return this;
    }

    public FluentRenderTreeBuilder AddMarkupContent(string? markupContent)
    {
        _builder.AddMarkupContent(GetSequence(), markupContent);
        return this;
    }


    #region AddContent

    public FluentRenderTreeBuilder AddContent(string? textContent)
    {
        _builder.AddContent(GetSequence(), textContent);
        return this;
    }

    public FluentRenderTreeBuilder AddContent(RenderFragment? fragment)
    {
        _builder.AddContent(GetSequence(), fragment);
        return this;
    }

    public FluentRenderTreeBuilder AddContent<TValue>(RenderFragment<TValue>? fragment, TValue value)
    {
        _builder.AddContent<TValue>(GetSequence(), fragment, value);
        return this;
    }


    public FluentRenderTreeBuilder AddContent(MarkupString? markupContent)
        => AddMarkupContent(markupContent?.Value);

    public FluentRenderTreeBuilder AddContent(MarkupString markupContent)
         => AddMarkupContent(markupContent.Value);

    public FluentRenderTreeBuilder AddContent(object? textContent)
        => AddContent(textContent?.ToString());

    #endregion


    public FluentRenderTreeBuilder AddAttribute(string name)
    {
        _builder.AddAttribute(GetSequence(), name);
        return this;
    }

    public FluentRenderTreeBuilder AddAttribute(string name, bool value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }

    public FluentRenderTreeBuilder AddAttribute(string name, string? value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }


    public FluentRenderTreeBuilder AddAttribute(string name, MulticastDelegate? value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }

    public FluentRenderTreeBuilder AddAttribute(string name, EventCallback value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }

    public FluentRenderTreeBuilder AddAttribute<TArgument>(string name, EventCallback<TArgument> value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }

    public FluentRenderTreeBuilder AddAttribute(string name, object? value)
    {
        _builder.AddAttribute(GetSequence(), name, value);
        return this;
    }

    public FluentRenderTreeBuilder AddMultipleAttributes(IEnumerable<KeyValuePair<string, object>>? attributes)
    {
        _builder.AddMultipleAttributes(GetSequence(), attributes);
        return this;
    }


    public FluentRenderTreeBuilder SetKey(object? value)
    {
        _builder.SetKey(value);
        return this;
    }


    public FluentRenderTreeBuilder AddElementReferenceCapture(Action<ElementReference> elementReferenceCaptureAction)
    {
        _builder.AddElementReferenceCapture(GetSequence(), elementReferenceCaptureAction);
        return this;
    }

    public FluentRenderTreeBuilder OpenRegion()
    {
        _builder.OpenRegion(GetSequence());
        return this;
    }

    public FluentRenderTreeBuilder CloseRegion()
    {
        _builder.CloseRegion();
        return this;
    }

    public FluentRenderTreeBuilder Clear()
    {
        _builder.Clear();
        return this;
    }
}
