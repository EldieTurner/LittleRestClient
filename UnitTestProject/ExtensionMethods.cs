using Newtonsoft.Json;

namespace UnitTestProject
{
    internal static class ExtensionMethods
    {
        public static string ToJsonString(this object o)
            => JsonConvert.SerializeObject(o);

        public static T FromJsonString<T>(this string s)
            => JsonConvert.DeserializeObject<T>(s);
    }
}
