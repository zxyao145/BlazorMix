namespace BlazorMix;

public partial class SwiperItem : BmDomComponentBase
{
    public const string ClsPrefix = "bm-swiper-item";

    /// <summary>
    /// 点击滑块时触发	
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    internal Swiper Parent { get; set; }

    internal StyleBuilder TransfromStyle { get; } = new StyleBuilder();

    protected override void OnInitialized()
    {
        Parent.AddSwiperItem(this);
        _classBuilder.Clear()
            .Add(ClsPrefix)
            .Add(()=> Class?.ToString() ?? "")
            ;
        _styleBuilder.Clear()
            .Add(() => Style?.ToString() ?? "")
            ;
    }

    internal string GetMergedStyle()
    {
        return $"{Style?.ToString() ?? ""}; {TransfromStyle.ToString()}";
    }


    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

}
