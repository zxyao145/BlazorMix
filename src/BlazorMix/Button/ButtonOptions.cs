namespace BlazorMix;

/// <summary>
/// Button Options
/// </summary>
public class ButtonOptions
{
    internal static RenderFragment DefaultLoadingIcon = (builder) =>
    {
        builder.Fluent()
            .OpenComponent<LoadingIcon>()
            .CloseComponent();
    };

    /// <inheritdoc cref="Button.Block"/>
    public bool Block { get; set; }

    /// <inheritdoc cref="Button.Disabled"/>
    public bool Disabled { get; set; }

    /// <inheritdoc cref="Button.Loading"/>
    public bool Loading { get; set; }

    /// <inheritdoc cref="Button.AutoLoading"/>
    public bool AutoLoading { get; set; }

    /// <inheritdoc cref="Button.Color"/>
    public string Color { get; set; } = ButtonColor.Default;

    /// <inheritdoc cref="Button.Shape"/>
    public string Shape { get; set; } = ButtonShape.Rounded;

    /// <inheritdoc cref="Button.Type"/>
    public string Type { get; set; } = ButtonType.Button;

    /// <inheritdoc cref="Button.Size"/>
    public string Size { get; set; } = ButtonSize.Medium;

    /// <inheritdoc cref="Button.Fill"/>
    public string Fill { get; set; } = ButtonFillType.Solid;

    /// <inheritdoc cref="Button.Icon"/>
    public RenderFragment? Icon { get; set; }

    /// <inheritdoc cref="Button.LoadingIcon"/>
    public RenderFragment LoadingIcon { get; set; } = DefaultLoadingIcon;

    /// <inheritdoc cref="Button.OnClick
    /// "/>
    public EventCallback<MouseEventArgs> OnClick { get; set; }
}


