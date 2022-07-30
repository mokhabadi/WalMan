using System.Diagnostics;
using WalMan.Properties;

namespace WalMan
{
    internal static class Manager
    {
        static Task? commandTask;
        static AsyncTimer? asyncTimer;
        static List<string> filePathList = new();
        static Dictionary<string, Command>? commandDictionary;

        static Settings Settings => Settings.Default;
        static string WallpaperPath => Application.StartupPath + "Wallpaper.bmp";
        public static string[] TimeIntervals => timeIntervalDictionary.Keys.ToArray();

        public static readonly List<Command> commandList = new()
        {
            new Command(Reload),
            new Command(Next),
            new Command(Skip),
            new Command(Delete),
            new Command(MarkAsMasterpiece, "Mark as Masterpiece"),
            new Command(ShowInExplorer, "Show in Explorer"),
        };

        public static readonly Dictionary<string, int> timeIntervalDictionary = new()
        {
            { "10 seconds", 10 },
            { "1 hour", 3600 },
            { "2 hour", 7200 },
            { "3 hour", 10800 },
        };

        public static void Load()
        {
            WindowsRegistry.EnableFeatures();
            NamedPipeStream.OnReceive += ExecuteCommand;
            NamedPipeStream.Receive();
            Application.ApplicationExit += ApplicationExit;

            if (Settings.skips == null)
                Settings.skips = Array.Empty<string>();

            if (Settings.currentWallpaper != "")
                StartTimer(Settings.timerInterval - Settings.elapsedTime);
            else if (Settings.wallpaperFolder != "")
                ChangeWallpaperFolder(Settings.wallpaperFolder);
        }

        public static void StartTimer(int timerInterval)
        {
            asyncTimer = new AsyncTimer(timerInterval);
            asyncTimer.Elapsed += () => ExecuteCommand(nameof(Next));
        }

        public static void ExecuteCommand(string commandName)
        {
            if (commandTask?.IsCompleted == false)
                return;

            if (commandName == "Open")
            {
                Open();
                return;
            }

            if (commandName == "Exit")
            {
                Application.Exit();
                return;
            }

            if (Settings.wallpaperFolder == "" || Directory.Exists(Settings.wallpaperFolder) == false)
                return;

            if (commandDictionary == null)
            {
                commandDictionary = new Dictionary<string, Command>();

                foreach (Command command in commandList)
                    commandDictionary.Add(command.Name, command);
            }

            if (commandDictionary.ContainsKey(commandName))
                commandTask = commandDictionary[commandName].Action();
        }

        public static void ChangeWallpaperFolder(string wallpaperFolder)
        {
            Settings.wallpaperFolder = wallpaperFolder;
            Settings.elapsedTime = 0;
            ExecuteCommand(nameof(Next));
        }

        public static void IntervalChanged(string intervalName)
        {
            Settings.timerInterval = timeIntervalDictionary[intervalName];
        }

        static async Task UpdateFilePathList()
        {
            await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(Settings.wallpaperFolder);
                filePathList = new List<string>(filePaths);
                filePathList.Sort();
            });
        }

        static async Task Reload()
        {
            if (File.Exists(Settings.currentWallpaper) == false)
                return;

            Stream? stream = await WalCreator.Create(Settings.currentWallpaper);

            if (stream != null)
                await SetWallpaper(stream);
        }

        static async Task Next()
        {
            if (Settings.wallpaperFolder == "" || Directory.Exists(Settings.wallpaperFolder) == false)
                return;

            await UpdateFilePathList();
            int wallpaperIndex = filePathList.IndexOf(Settings.currentWallpaper);
            bool result = await SetWallpaper(wallpaperIndex);

            if (result == true)
                StartTimer(Settings.timerInterval);
        }

        static async Task<bool> SetWallpaper(int wallpaperIndex)
        {
            List<string> skipList = new(Settings.skips);
            int startIndex = wallpaperIndex;

            while (true)
            {
                wallpaperIndex++;

                if (wallpaperIndex == filePathList.Count && startIndex == -1)
                    return false;

                if (wallpaperIndex == filePathList.Count)
                    wallpaperIndex = 0;

                if (wallpaperIndex == startIndex)
                    return false;

                if (skipList.Contains(filePathList[wallpaperIndex]))
                    continue;

                Stream? stream = await WalCreator.Create(filePathList[wallpaperIndex]);

                if (stream != null)
                {
                    await SetWallpaper(stream);
                    Settings.currentWallpaper = filePathList[wallpaperIndex];
                    Settings.Save();
                    return true;
                }
            }
        }

        static async Task SetWallpaper(Stream stream)
        {
            FileStream fileStream = new(WallpaperPath, FileMode.Create, FileAccess.ReadWrite);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
            await DesktopBackground.Set(WallpaperPath, DesktopBackground.Style.Center);
            await Task.Run(() => File.Delete(WallpaperPath));
        }

        static async Task Skip()
        {
            List<string> skipList = new(Settings.skips);
            skipList.Add(Settings.currentWallpaper);
            Settings.skips = skipList.ToArray();
            await Next();
        }

        static async Task Delete()
        {
            if (File.Exists(Settings.currentWallpaper) == false)
                return;

            string fileToDelete = Settings.currentWallpaper;
            await Next();
            await Task.Run(() => File.Delete(fileToDelete));
        }

        static async Task MarkAsMasterpiece()
        {
            if (File.Exists(Settings.currentWallpaper) == false)
                return;

            string directory = Path.GetDirectoryName(Settings.currentWallpaper) + @"\Masterpiece";

            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            string filePath = directory + @"\" + Path.GetFileName(Settings.currentWallpaper);

            if (File.Exists(filePath) == false)
                await Task.Run(() => File.Copy(Settings.currentWallpaper, filePath));
        }

        static async Task ShowInExplorer()
        {
            if (File.Exists(Settings.currentWallpaper) == false)
                return;

            await Task.Run(() => Process.Start("explorer.exe", "/select,\"" + Settings.currentWallpaper + "\""));
        }

        static void ApplicationExit(object? sender, EventArgs e)
        {
            if (asyncTimer != null)
                Settings.elapsedTime = asyncTimer.ElapsedTime;
        }
        static MainForm mainForm;
        public static void Open()
        {
            mainForm = new();
            mainForm.WallpaperFolderChanged += ChangeWallpaperFolder;
            mainForm.IntervalChanged += IntervalChanged;
            mainForm.Loaded += MainFormLoaded;
            mainForm.DisableClicked += MainFormDisableClicked;
            mainForm.ShowDialog();
        }

        private static void MainFormDisableClicked()
        {
            WindowsRegistry.DisableFeatures();
        }

        static void MainFormLoaded()
        {
            mainForm.Initialize(Settings.wallpaperFolder,CurrentInterval());
        }

        static string? CurrentInterval()
        {
            foreach (string intervalName in TimeIntervals)
                if (timeIntervalDictionary[intervalName] == Settings.timerInterval)
                    return intervalName;

            return null;
        }
    }
}
