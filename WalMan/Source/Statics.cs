using System.Text.RegularExpressions;

namespace WalMan
{
    public static class Statics
    {
        public static string Enquote(this string value)
        {
            return $"\"{value}\"";
        }

        public static string SecondToString(int seconds)
        {
            if (seconds < 120)
                return $"{seconds} Seconds";

            int minutes = seconds / 60;

            if (minutes < 120)
                return $"{minutes} Minutes";

            int hours = minutes / 60;

            if (minutes % 60 == 0)
                return $"{hours} Hours";

            return $"{hours} Hours {minutes % 60} Minutes";
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }

    }
}
