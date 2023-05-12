using static System.Net.Mime.MediaTypeNames;

namespace BlazorMix;

public struct StringOrRenderFragment
{
    public RenderFragment? Node { get; private set; }
    public string OriginText { get; private set; } = "";

    public bool IsText { get; private set; } =false;

    public StringOrRenderFragment(string text)
    {
        Node = builder =>
        {
            builder.Fluent().AddContent(text);
        };
        OriginText = text;
        IsText = true;
    }

    public StringOrRenderFragment(RenderFragment? renderFragment)
    {
        Node = renderFragment;
    }

    public static implicit operator StringOrRenderFragment(string text) => new(text);

    public static implicit operator StringOrRenderFragment(RenderFragment? renderFragment) => new(renderFragment);


}
