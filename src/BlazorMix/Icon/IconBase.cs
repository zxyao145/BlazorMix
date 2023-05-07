namespace BlazorMix;
public class IconBase : ComponentBase
{
    [Parameter]
    public string Width { get; set; } = "1em";

    [Parameter]
    public string Height { get; set; } = "1em";
}
