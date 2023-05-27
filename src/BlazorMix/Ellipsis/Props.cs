
namespace BlazorMix;
public class EllipsisProps
{
    public string Content { get; set; } = "";

    public string Direction { get; set; }

    public int Rows { get; set; } = 1;

    public string ExpandText { get; set; } = "";

    public string CollapseText { get; set; } = "";

    public bool Expanded { get; set; }
}

public class CalcResult
{
    public bool Exceeded { get; set; }

    public string Leading { get; set; } = "";

    public string Tailing { get; set; } = "";

}

