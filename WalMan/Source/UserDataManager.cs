using System.IO;
using System.Text.Json;

namespace WalMan.Settings
{
    internal static class UserDataManager
    {
        static string FileName => nameof(UserData);
        public static UserData UserData { get; private set; }

        static UserDataManager()
        {
            UserData = new UserData();
            UserData.DateChange += Save;
        }

        public static async void Load()
        {
            if (File.Exists(FileName) == false)
                return;

            using FileStream fileStream = File.OpenRead(FileName);
            UserData? userData = await JsonSerializer.DeserializeAsync<UserData?>(fileStream);

            if (userData != null)
            {
                UserData = userData;
                UserData.DateChange += Save;
            }
        }

        static async void Save()
        {
            using FileStream fileStream = File.Exists(FileName) ? File.Open(FileName, FileMode.Truncate) : File.OpenWrite(FileName);
            await JsonSerializer.SerializeAsync(fileStream, UserData);
            await fileStream.DisposeAsync();
        }
    }
}
