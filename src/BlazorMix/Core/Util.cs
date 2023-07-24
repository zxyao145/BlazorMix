

namespace BlazorMix;
public class Util
{
    #region Nearest

    public static int Nearest(List<int> arr, int target)
    {
        return arr.Aggregate(
            (pre, cur) => Math.Abs(pre - target) < Math.Abs(cur - target)
                ? pre
                : cur
        );
    }

    public static int Nearest(int[] arr, int target)
    {
        return arr.Aggregate(
            (pre, cur) => Math.Abs(pre - target) < Math.Abs(cur - target)
                ? pre
                : cur
        );
    }

    public static double Nearest(List<double> arr, double target)
    {
        return arr.Aggregate(
            (pre, cur) => Math.Abs(pre - target) < Math.Abs(cur - target)
                ? pre
                : cur
                );
    }

    public static double Nearest(double[] arr, double target)
    {
        return arr.Aggregate(
            (pre, cur) => Math.Abs(pre - target) < Math.Abs(cur - target)
                ? pre
                : cur
        );
    } 
    
    #endregion
}
