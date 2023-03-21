using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WalMan
{
    internal static class Log
    {
        public static readonly string fileName = "Log.txt";
        static readonly ConcurrentQueue<string> queue = new();
        static Task task = Task.CompletedTask;

        public static void Wait()
        {
            task.Wait();
        }

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
            StringBuilder stringBuilder = new();

            while (queue.TryDequeue(out string? nextMessage))
                stringBuilder.Append(nextMessage);

            await File.AppendAllTextAsync(fileName, stringBuilder.ToString());

            if (!queue.IsEmpty)
                await Save();
        }
    }
}
