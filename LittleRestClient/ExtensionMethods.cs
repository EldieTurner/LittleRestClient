using System;

namespace LittleRestClient
{
    [ExcludeFromCoverage]
    internal static class Extensionmethods
    {
        public static bool IsNullOrWhiteSpace(this string str) 
            => String.IsNullOrWhiteSpace(str);
        public static string WhiteSpaceIsNull(this string str)
            => str.IsNullOrWhiteSpace() ? null : str; 
    }
}
