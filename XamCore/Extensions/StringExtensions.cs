using System;
using XamCore.View;

namespace XamCore.Extensions
{
    public static class StringExtensions
    {
        public static string TrimAllWhitespace(this string input) => input
            .Replace("\n", "")
            .Replace("\t", "")
            .Replace(" ", "");

        public static string ToNavigationTypeString(
            this string? navigationLink,
            Type? defaultType = null)
        {
            defaultType ??= typeof(TransparentNavigationPage);

            return navigationLink switch {
                _ => nameof(TransparentNavigationPage)
            };
        }
    }
}
