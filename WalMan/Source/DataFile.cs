using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WalMan
{
    internal class DataFile
    {
        public static async Task<T?> QuickLoad<T>()
        {
            string fileName = typeof(T).Name;

            if (File.Exists(fileName) == false)
                return default;

            using FileStream fileStream = File.OpenRead(fileName);
            T? dataObject = await JsonSerializer.DeserializeAsync<T>(fileStream);
            return dataObject;
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
