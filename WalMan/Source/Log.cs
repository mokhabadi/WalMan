namespace WalMan
{
    internal static class Log
    {
        public static async Task Add(string message)
        {
            string filePath = Application.StartupPath + "Log.txt";
            DateTime dateTime = DateTime.Now;
            message = $"[{dateTime}.{dateTime.Millisecond}] {message}";
            await File.AppendAllTextAsync(filePath, message);
        }
    }
}
