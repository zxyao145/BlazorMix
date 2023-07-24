namespace BlazorMix;

public struct IndicatorInfo
{
    public IndicatorInfo(int total = 0, int current = 0)
    {
        this.Total = total;
        this.Current = current;
    }

    public int Total { get; set; }
    public int Current { get; set; }
}