
namespace BlazorMix;


public class ButtonSize
{
    private static string _mini = BmSize.Mini.GetDisplayName();
    private static string _small = BmSize.Small.GetDisplayName();
    private static string _medium = BmSize.Medium.GetDisplayName();
    private static string _large = BmSize.Large.GetDisplayName();
    private static string _extraLarge = BmSize.ExtraLarge.GetDisplayName();

    public static string Mini => _mini;
    public static string Smalll => _small;

    /// <summary>
    /// normal, default
    /// </summary>
    public static string Medium => _medium;
    public static string Large => _large;

    public static string ExtraLarge => _extraLarge;
}

public class ButtonShape
{
    public static string Rounded => BorderRadius.Rounded;
    public static string Rectangular => BorderRadius.Rectangular;
    public static string Circle => BorderRadius.Circle;
    public static string Capsule => BorderRadius.Capsule;
}


public class ButtonType
{
    public static string Button => "button";
    public static string Reset => "reset";
    public static string Submit => "submit";
}

public class ButtonFillType
{
    public static string Solid => "solid";
    public static string Outline => "outline";
    public static string None => "none";
}
public class ButtonColor: BmColor
{

}

