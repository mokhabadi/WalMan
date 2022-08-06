using System.Diagnostics;
using System.Runtime.InteropServices;
using WalMan.Properties;

namespace WalMan
{
    internal static class Manager
    {
        static Task? commandTask;
        static AsyncTimer? asyncTimer;
        static List<string> filePathList = new();
        static Dictionary<string, Command>? commandDictionary;
        static readonly MainForm mainForm = new();
        public static readonly int[] timeIntervals = { 10, 30, 60, 180, 600, 1800, 3600, 7200, 10800 };

        static Settings Settings => Settings.Default;
        static string WallpaperPath => Application.StartupPath + "Wallpaper.bmp";
        public static Command[] Commands => commandList.ToArray();

        static readonly List<Command> commandList = new()
        {
            new Command(Reload),
            new Command(Next),
            new Command(Skip),
            new Command(Delete),
            new Command(MarkAsMasterpiece, "Mark as Masterpiece"),
            new Command(ShowInExplorer, "Show in Explorer"),
        };

        public static void Load()
        {
            Log.Add("<-------------------------------------Load------------------------------------->");
            WindowsRegistry.EnableFeatures();
            NamedPipeStream.OnReceive += ExecuteCommand;
            NamedPipeStream.Receive();
            Application.ApplicationExit += ApplicationExit;
            mainForm.WallpaperFolderChanged += ChangeWallpaperFolder;
            mainForm.IntervalChanged += IntervalChanged;
            mainForm.Loaded += MainFormLoaded;
            mainForm.DisableClicked += MainFormDisableClicked;

            if (Settings.skips == null)
                Settings.skips = Array.Empty<string>();

            if (Settings.currentWallpaper != "")
                StartTimer(Settings.remainingTime);
            else if (Settings.wallpaperFolder != "")
                ChangeWallpaperFolder(Settings.wallpaperFolder);
        }

        public static void StartTimer(int timerInterval)
        {
            Log.Add("StartTimer: " + timerInterval);
            asyncTimer = new AsyncTimer(timerInterval, () => ExecuteCommand(nameof(Next)));
        }

        public static void ExecuteCommand(string commandName)
        {
            Log.Add("ExecuteCommand: " + commandName);

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
            Log.Add("ChangeWallpaperFolder: " + wallpaperFolder);
            Settings.wallpaperFolder = wallpaperFolder;
            Settings.remainingTime = 0;
            ExecuteCommand(nameof(Next));
        }

        public static void IntervalChanged(int intervalIndex)
        {
            Log.Add("IntervalChanged: " + intervalIndex);
            Settings.intervalIndex = intervalIndex;
        }

        static async Task UpdateFilePathList()
        {
            await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(Settings.wallpaperFolder);
                filePathList = new(filePaths);
                filePathList.Sort((x, y) => StrCmpLogicalW(x, y));
            });
        }

        static async Task Reload()
        {
            if (File.Exists(Settings.currentWallpaper) == false)
                return;

            Stream? stream = await WalCreator.Create(Settings.currentWallpaper);

            if (stream != null)
                await SetDesktopBackground(stream);
        }

        static async Task Next()
        {
            if (Settings.wallpaperFolder == "" || Directory.Exists(Settings.wallpaperFolder) == false)
                return;

            await UpdateFilePathList();
            int wallpaperIndex = filePathList.IndexOf(Settings.currentWallpaper);
            bool result = await SetWallpaper(wallpaperIndex);

            if (result == true)
                StartTimer(timeIntervals[Settings.intervalIndex]);
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
                    await SetDesktopBackground(stream);
                    Settings.currentWallpaper = filePathList[wallpaperIndex];
                    Settings.Save();
                    return true;
                }
            }
        }

        static async Task SetDesktopBackground(Stream stream)
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
            Settings.remainingTime = 0;

            if (asyncTimer != null)
                Settings.remainingTime = asyncTimer.RemainingTime;
        }

        public static void Open()
        {
            mainForm.ShowDialog();
        }

        static void MainFormLoaded()
        {
            mainForm.Initialize(Settings.wallpaperFolder, Settings.intervalIndex, Settings.skips);
        }

        static void MainFormDisableClicked()
        {
            WindowsRegistry.DisableFeatures();
            Settings.Reset();
        }

        public static string GetRemaining()
        {
            if (asyncTimer != null)
                return SecondToString(asyncTimer.RemainingTime);

            return "Disabled";
        }

        public static string SecondToString(int seconds)
        {
            if (seconds < 120)
                return $"{seconds} Seconds";

            int minutes = seconds / 60;

            if (minutes < 120)
                return $"{minutes} Minutes";

            int hours = minutes / 60;

            return $"{hours} Hours {minutes % 60} Minutes";
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(string x, string y);

    }
}