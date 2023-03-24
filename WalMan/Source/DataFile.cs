using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WalMan
{
    internal class DataFile
    {
        public static bool Exists<T>()
        {
            return File.Exists(typeof(T).Name);
        }

        public static async Task<T?> QuickLoad<T>()
        {
            using FileStream fileStream = File.OpenRead(typeof(T).Name);
            return await JsonSerializer.DeserializeAsync<T>(fileStream);
        }

        public static async Task QuickSave<T>(T dataObject)
        {
            string fileName = typeof(T).Name;
            using FileStream fileStream = File.Open(fileName, FileMode.Create);
            await JsonSerializer.SerializeAsync(fileStream, dataObject);
            await fileStream.DisposeAsync();
        }
    }
}
