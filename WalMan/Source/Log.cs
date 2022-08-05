using System.Text;

namespace WalMan
{
    internal static class Log
    {
        static System.Collections.Concurrent.ConcurrentQueue<string> queue = new();
        static Task task = Task.CompletedTask;

        public static void Add(string message)
        {
            DateTime dateTime = DateTime.Now;
            message = $"[{dateTime}.{dateTime.Millisecond:D3}] {message}{Environment.NewLine}";
            queue.Enqueue(message);

            if (task.IsCompleted)
                task = Save();
        }

        public static async Task Save()
        {
            string filePath = Application.StartupPath + "Log.txt";
            StringBuilder stringBuilder = new();

            while (queue.TryDequeue(out string? nextMessage))
                stringBuilder.Append(nextMessage);

            await File.AppendAllTextAsync(filePath, stringBuilder.ToString());

            if (!queue.IsEmpty)
                await Save();
        }
    }
}
