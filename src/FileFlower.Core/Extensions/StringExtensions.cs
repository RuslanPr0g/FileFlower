using System.Text.RegularExpressions;

namespace FileFlower.Core.Extensions;

internal static class StringExtensions
{
    public static bool Like(this string fileName, string pattern)
    {
        var regex = "^" + Regex.Escape(pattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".") + "$";
        return Regex.IsMatch(fileName, regex, RegexOptions.IgnoreCase);
    }
}
