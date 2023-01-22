namespace WalMan
{
    internal static class Statics
    {
        public static string Enquote(this string value)
        {
            return $"\"{value}\"";
        }
    }
}
