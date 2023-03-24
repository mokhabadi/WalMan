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
    public class Manager
    {
        static readonly string BackgroundFileName = "Background.bmp";
        readonly Map<string, Command> commandMap = new();
        readonly MainForm mainForm = new();
        UserData userData = new();
        Task task = Task.CompletedTask;
        AsyncTimer? asyncTimer;

        public async void Initialize()
        {
            if (DataFile.Exists<UserData>() == false)
                await DataFile.QuickSave(userData);

            userData = await DataFile.QuickLoad<UserData>() ?? new UserData();
            userData.WallpaperChange += CreateMenus;
            userData.DateChange += Save;
            CreateMenus();
            NamedPipeStream.OnReceive += ExecuteCommand;
            NamedPipeStream.Receive();
            Application.ApplicationExit += ApplicationExit;
            SystemEvents.SessionEnding += SystemEventsSessionEnding;
            mainForm.WallpaperChange += WallpaperChanged;
            mainForm.IntervalChanged += IntervalChanged;

            if (userData.Wallpaper != null && userData.Interval > 0 && userData.RemainingTime > 0)
                asyncTimer = new AsyncTimer(userData.RemainingTime, () => ExecuteCommand(nameof(Next)));
        }

        async void Save()
        {
            if (task.IsCompleted == false)
                await task;

            task = DataFile.QuickSave(userData);
        }

        void CreateMenus()
        {
            Command[] commands = CreateCommands();
            commandMap.Fill(commands, command => command.Id);
            MainApplicationContext.CreateMenu(commands);
            //WindowsRegistry.CreateMenu(commands);
        }

        Command[] CreateCommands()
        {
            List<Command> commandList = new();

            if (userData.Wallpaper != null)
            {
                commandList.Add(new(OpenFile, userData.Wallpaper));
                commandList.Add(new(Reload));
                commandList.Add(new(Next));
                commandList.Add(new(Skip));
                commandList.Add(new(Delete));
                commandList.Add(new(ShowInExplorer));
            }
            else
                commandList.Add(new(SelectWallpaper));

            commandList.Add(new(Settings));
            commandList.Add(new(Exit));
            return commandList.ToArray();
        }

        Task OpenFile()
        {
            Statics.OpenFile(userData.Wallpaper);
            return Task.CompletedTask;
        }

        void SystemEventsSessionEnding(object sender, SessionEndingEventArgs sessionEndingEventArgs)
        {
            Log.Add("SystemEventsSessionEnding");
            Application.Exit();
        }

        public void ExecuteCommand(string commandId)
        {
            Log.Add($"ExecuteCommand: {commandId}");

            if (task.IsCompleted == false)
                return;

            if (commandMap.HasKey(commandId))
                task = commandMap[commandId].Action();
        }

        public void WallpaperChanged(string? fileName)
        {
            userData.SetWallpaper(fileName);
            Log.Add($"Wallpaper selected: {fileName ?? "disabled"}");

            if (fileName != null)
                ExecuteCommand(nameof(Reload));
        }

        public void IntervalChanged(int interval)
        {
            if (userData.Interval == interval)
                return;

            Log.Add($"IntervalChanged: {interval}");
            userData.SetInterval(interval);
            asyncTimer?.Stop();

            if (interval > 0)
                asyncTimer = new AsyncTimer(interval, () => ExecuteCommand(nameof(Next)));
        }

        async Task Reload()
        {
            if (File.Exists(userData.Wallpaper) == false)
            {
                MessageBox.Show("Can't find current wallpaper!");
                return;
            }

            Stream? stream = await WalCreator.Create(userData.Wallpaper);

            if (stream != null)
                await SetDesktopBackground(stream);
        }

        async Task Next()
        {
            if (Directory.Exists(Path.GetDirectoryName(userData.Wallpaper)) == false)
            {
                MessageBox.Show("Can't find current wallpaper directory!");
                return;
            }

            string[] filePaths = Directory.GetFiles(Path.GetDirectoryName(userData.Wallpaper));
            List<string> filePathList = new(filePaths);
            filePathList.Sort((x, y) => StrCmpLogicalW(x, y));
            bool result = await SetWallpaper(filePathList);
            asyncTimer?.Stop();

            if (result == true && userData.Interval > 0)
                asyncTimer = new AsyncTimer(userData.Interval, () => ExecuteCommand(nameof(Next)));
        }

        async Task<bool> SetWallpaper(List<string> filePathList)
        {
            int wallpaperIndex = filePathList.IndexOf(userData.Wallpaper);
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
                    userData.SetWallpaper(filePathList[wallpaperIndex]);
                    return true;
                }
            }
        }

        static async Task SetDesktopBackground(Stream stream)
        {
            FileStream fileStream = new(BackgroundFileName, FileMode.Create, FileAccess.ReadWrite);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
            DesktopBackground.Set(Path.GetFullPath(BackgroundFileName), DesktopBackground.Style.Center);
        }

        async Task Skip()
        {
            userData.AddSkip();
            await Next();
        }

        async Task Delete()
        {
            string? fileToDelete = userData.Wallpaper;
            await Next();

            if (File.Exists(fileToDelete))
                FileSystem.DeleteFile(fileToDelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }

        Task MarkAsMasterpiece()
        {
            if (File.Exists(userData.Wallpaper) == false)
            {
                MessageBox.Show("File not found!");
                return Task.CompletedTask;
            }

            string directory = @$"{Path.GetDirectoryName(userData.Wallpaper)}\Masterpiece";

            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            string filePath = $@"{directory}\{Path.GetFileName(userData.Wallpaper)}";

            if (File.Exists(filePath) == false)
                File.Copy(userData.Wallpaper, filePath);

            return Task.CompletedTask;
        }

        Task ShowInExplorer()
        {
            if (File.Exists(userData.Wallpaper))
                Process.Start("explorer.exe", $"/select,\"{userData.Wallpaper}\"");

            return Task.CompletedTask;
        }

        void ApplicationExit(object? sender, EventArgs e)
        {
            userData.SetRemainingTime(asyncTimer == null ? 0 : asyncTimer.RemainingTime());
            Log.Add(@"\----- ApplicationExit -----/");
            Log.Wait();
        }

        Task SelectWallpaper()
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                userData.SetWallpaper(openFileDialog.FileName);

            return Task.CompletedTask;
        }

        public Task Settings()
        {
            mainForm.Initialize(userData);
            mainForm.ShowDialog();
            return Task.CompletedTask;
        }

        public int GetRemainingTime()
        {
            if (asyncTimer == null)
                return 0;

            return asyncTimer.RemainingTime();
        }

        Task Exit()
        {
            Application.Exit();
            return Task.CompletedTask;
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(string x, string y);
    }
}
