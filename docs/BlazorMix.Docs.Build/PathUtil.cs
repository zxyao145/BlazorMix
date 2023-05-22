using System;

namespace BlazorMix.Docs.Build;
internal class PathUtil
{
    /// <summary>
    /// 计算相对路径
    /// 后者相对前者的路径。
    /// </summary>
    /// <param name="mainDir">主目录</param>
    /// <param name="fullFilePath">文件的绝对路径</param>
    /// <returns>fullFilePath相对于mainDir的路径</returns>
    /// <example>
    /// @"..\..\regedit.exe" = GetRelativePath(@"D:\Windows\Web\Wallpaper\", @"D:\Windows\regedit.exe" );
    /// </example>
    public static string GetRelativePath(string mainDir, string fullFilePath)
    {
        mainDir = mainDir.Replace("\\", "/");
        fullFilePath = fullFilePath.Replace("\\", "/");
        if (!mainDir.EndsWith("/"))
        {
            mainDir += "/";
        }

        int intIndex = -1, intPos = mainDir.IndexOf('/');

        while (intPos >= 0)
        {
            intPos++;
            if (string.Compare(mainDir, 0, fullFilePath, 0, intPos, true) != 0) break;
            intIndex = intPos;
            intPos = mainDir.IndexOf('/', intPos);
        }

        if (intIndex >= 0)
        {
            fullFilePath = fullFilePath.Substring(intIndex);
            intPos = mainDir.IndexOf("/", intIndex, StringComparison.Ordinal);
            while (intPos >= 0)
            {
                fullFilePath = "../" + fullFilePath;
                intPos = mainDir.IndexOf("/", intPos + 1, StringComparison.Ordinal);
            }
        }

        return fullFilePath;
    }
}
