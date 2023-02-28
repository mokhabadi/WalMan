using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WalMan
{
    internal class Manager
    {
        Task? commandTask;
        AsyncTimer? asyncTimer;
        List<string> filePathList = new();
        readonly Map<string, Command> commandMap;
        readonly MainForm mainForm = new();
        static readonly string BackgroundFileName = "Background.bmp";
        public Command[] commands;
        UserData userData;
        readonly string[] fileNames = { Log.fileName, nameof(userData), BackgroundFileName };

        public Manager()
        {
            commands = new[]
            {
                new Command(Reload),
                new Command(Next),
                new Command(Skip),
                new Command(Delete),
                new Command(MarkAsMasterpiece, "Mark as Masterpiece"),
                new Command(ShowInExplorer, "Show in Explorer"),
                new Command(Settings),
                new Command(Exit),
            };

            commandMap = new();

            foreach (Command command in commands)
                commandMap.Add(command.Name, command);

            userData = new();
        }



        public async void Initialize()
        {
            Exception? exception = CheckFiles();

            if (exception != null)
            {
                MessageBox.Show("Can't create necessary files.\n please run WalMan as administrator!");
                return;
            }

            userData = await DataFile.QuickLoad<UserData>() ?? new();
            WindowsRegistry.EnableFeatures(commands);
            NamedPipeStream.OnReceive += ExecuteCommand;
            NamedPipeStream.Receive();
            Application.ApplicationExit += ApplicationExit;
            SystemEvents.SessionEnding += SystemEventsSessionEnding;
            mainForm.WallpaperFolderChanged += ChangeWallpaperFolder;
            mainForm.IntervalChanged += IntervalChanged;
            mainForm.DisableClicked += MainFormDisableClicked;

            if (userData.CurrentWallpaper != null)
                StartTimer(userData.RemainingTime);
        }

        Exception? CheckFiles()
        {
            try
            {
                foreach (string fileName in fileNames)
                    if (File.Exists(fileName) == false)
                        File.Create(fileName);
            }
            catch (Exception exception)
            {
                return exception;
            }

            return null;
        }

        void SystemEventsSessionEnding(object sender, SessionEndingEventArgs sessionEndingEventArgs)
        {
            Application.Exit();
        }

        public void StartTimer(int timeInterval)
        {
            Log.Add($"StartTimer: {timeInterval}");
            asyncTimer?.Stop();
            asyncTimer = new AsyncTimer(timeInterval, () => ExecuteCommand(nameof(Next)));
        }

        public void ExecuteCommand(string commandName)
        {
            Log.Add($"ExecuteCommand: {commandName}");

            if (commandTask?.IsCompleted == false)
                return;

            if (commandName == "Open")
            {
                Settings();
                return;
            }

            if (commandName == "Exit")
            {
                Application.Exit();
                return;
            }

            if (Directory.Exists(userData.WallpaperFolder) == false)
                return;

            if (commandMap.HasKey(commandName))
                commandTask = commandMap[commandName].Action();
        }

        public void ChangeWallpaperFolder(string wallpaperFolder)
        {
            Log.Add($"ChangeWallpaperFolder: {wallpaperFolder}");
            userData.SetWallpaperFolder(wallpaperFolder);
            userData.SetRemainingTime(0);
            ExecuteCommand(nameof(Next));
        }

        public void IntervalChanged(int interval)
        {
            if (userData.Interval == interval)
                return;

            Log.Add($"IntervalChanged: {interval}");
            userData.SetInterval(interval);
        }

        async Task UpdateFilePathList()
        {
            await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(userData.WallpaperFolder);
                filePathList = new(filePaths);
                filePathList.Sort((x, y) => StrCmpLogicalW(x, y));
            });
        }

        async Task Reload()
        {
            if (File.Exists(userData.CurrentWallpaper) == false)
                return;

            Stream? stream = await WalCreator.Create(userData.CurrentWallpaper);

            if (stream != null)
                await SetDesktopBackground(stream);
        }

        async Task Next()
        {
            if (Directory.Exists(userData.WallpaperFolder) == false)
                return;

            await UpdateFilePathList();
            int wallpaperIndex = filePathList.IndexOf(userData.CurrentWallpaper);
            bool result = await SetWallpaper(wallpaperIndex);

            if (result == true)
                StartTimer(userData.Interval);
        }

        async Task<bool> SetWallpaper(int wallpaperIndex)
        {
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

                if (userData.SkipsContain(filePathList[wallpaperIndex]))
                    continue;

                Stream? stream = await WalCreator.Create(filePathList[wallpaperIndex]);

                if (stream != null)
                {
                    await SetDesktopBackground(stream);
                    userData.SetCurrentWallpaper(filePathList[wallpaperIndex]);
                    return true;
                }
            }
        }

        async Task SetDesktopBackground(Stream stream)
        {
            FileStream fileStream = new(BackgroundFileName, FileMode.Create, FileAccess.ReadWrite);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
            await DesktopBackground.Set(BackgroundFileName, DesktopBackground.Style.Center);
            await Task.Run(() => File.Delete(BackgroundFileName));
        }

        async Task Skip()
        {
            userData.AddSkip();
            await Next();
        }

        async Task Delete()
        {
            string? fileToDelete = userData.CurrentWallpaper;
            await Next();

            if (File.Exists(fileToDelete))
                await Task.Run(() => FileSystem.DeleteFile(fileToDelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin));
        }

        async Task MarkAsMasterpiece()
        {
            if (File.Exists(userData.CurrentWallpaper) == false)
                return;

            string directory = @$"{Path.GetDirectoryName(userData.CurrentWallpaper)}\Masterpiece";

            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            string filePath = $@"{directory}\{Path.GetFileName(userData.CurrentWallpaper)}";

            if (File.Exists(filePath) == false)
                await Task.Run(() => File.Copy(userData.CurrentWallpaper, filePath));
        }

        async Task ShowInExplorer()
        {
            if (File.Exists(userData.CurrentWallpaper))
                await Task.Run(() => Process.Start("explorer.exe", $"/select,\"{userData.CurrentWallpaper}\""));
        }

        void ApplicationExit(object? sender, EventArgs e)
        {
            userData.SetRemainingTime(asyncTimer == null ? 0 : asyncTimer.RemainingTime());
            Log.Add(@"\----- ApplicationExit -----/");
            Log.Wait();
        }

        public Task Settings()
        {
            mainForm.Initialize(userData.WallpaperFolder, userData.Interval, userData.Skips);
            mainForm.ShowDialog();
            return Task.CompletedTask;
        }

        void MainFormDisableClicked()
        {
            WindowsRegistry.DisableFeatures();
            userData.Reset();
        }

        public string GetRemainingTime()
        {
            if (asyncTimer != null)
                return SecondToString(asyncTimer.RemainingTime());

            return "Disabled";
        }

        Task Exit()
        {
            Application.Exit();
            return Task.CompletedTask;
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

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(string x, string y);

    }
}