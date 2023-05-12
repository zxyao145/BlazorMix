namespace BlazorMix;

public class ActionInfo
{
    public string Key { get; set; } = Guid.NewGuid().ToString();

    public StringOrRenderFragment Text { get; set; } = "";
    
    public bool Disabled { get; set; } = false;

    public StringOrRenderFragment? Description { get; set; } 

    public bool Danger { get; set; } = false;

    public bool Bold { get; set; } = false;

    public Func<Task>? OnClick { get; set; }
}