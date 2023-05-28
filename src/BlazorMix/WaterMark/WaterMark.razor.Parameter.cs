namespace BlazorMix;

public class WaterMarkProps
{
    public WaterMarkProps(WaterMark component)
    {
        this.Content = component.Content.IsT0
            ? new List<string>() { component.Content.AsT0 }
            : component.Content.AsT1;
        this.FontColor = component.FontColor;
        this.FontSize = component.FontSize.IsT0 ? $"{component.FontSize.AsT0}px" : component.FontSize.AsT1;
        this.FontStyle = component.FontStyle.GetDisplayName();
        this.FontWeight = component.FontWeight;
        this.FontFamily = component.FontFamily;
        this.GapX = component.GapX;
        this.GapY = component.GapY;
        this.Width = component.Width;
        this.Height = component.Height;
        this.Rotate = component.Rotate;
        this.Image = component.Image;
        this.ImageWidth = component.ImageWidth;
        this.ImageHeight = component.ImageHeight;
    }

    public int GapX { get; set; }

    public int GapY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Rotate { get; set; } 

    public string Image { get; set; }

    public int ImageWidth { get; set; } 

    public int ImageHeight { get; set; }

    public List<string> Content { get; set; }

    public string FontColor { get; set; }

    public string FontStyle { get; set; }

    public int FontWeight { get; set; } 

    public string FontFamily { get; set; } 

    public string FontSize { get; set; }
}

public partial class WaterMark
{
    /// <summary>
    /// 水印之间的水平间距	
    /// </summary>
    [Parameter]
    public int GapX { get; set; } = 24;

    /// <summary>
    /// 水印之间的垂直间距	
    /// </summary>
    [Parameter]
    public int GapY { get; set; } = 48;

    /// <summary>
    /// 追加的水印元素的 z-index	
    /// </summary>
    [Parameter]
    public int ZIndex { get; set; } = 2000;

    /// <summary>
    /// 水印的宽度	
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 120;

    /// <summary>
    /// 水印的高度	
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 64;

    /// <summary>
    /// 水印绘制时，旋转的角度，单位 °	
    /// </summary>
    [Parameter]
    public int Rotate { get; set; } = -22;

    /// <summary>
    /// 图片源，建议导出 2 倍或 3 倍图，优先使用图片渲染水印	
    /// </summary>
    [Parameter]
    public string Image { get; set; } = "";

    /// <summary>
    /// 图片宽度	
    /// </summary>
    [Parameter]
    public int ImageWidth { get; set; } = 120;

    /// <summary>
    /// 图片高度	
    /// </summary>
    [Parameter]
    public int ImageHeight { get; set; } = 64;

    /// <summary>
    /// 水印文字内容	
    /// </summary>
    [Parameter]
    public OneOf<string, List<string>> Content { get; set; } = "";

    /// <summary>
    /// 水印文字颜色	
    /// </summary>
    [Parameter]
    public string FontColor { get; set; } = "rgba(0,0,0,.15)";

    /// <summary>
    /// 水印文字样式
    /// </summary>
    [Parameter]
    public FontStyle FontStyle { get; set; }

    /// <summary>
    /// 水印文字粗细
    /// </summary>
    [Parameter]
    public int FontWeight { get; set; } = 400;

    /// <summary>
    /// 水印文字的字体名
    /// </summary>
    [Parameter]
    public string FontFamily { get; set; } = "sans-serif";

    /// <summary>
    ///  文字大小	
    /// </summary>
    [Parameter]
    public OneOf<int, string> FontSize { get; set; } = 14;

    /// <summary>
    /// 是否覆盖整个页面	
    /// </summary>
    [Parameter]
    public bool FullPage { get; set; } = true;
}
