using System;
using System.Collections.Generic;
using System.IO;

namespace WalMan
{
    internal class UserData
    {
        public int Version { get; private set; }
        public int Interval { get; private set; }
        public int RemainingTime { get; private set; }
        public string? WallpaperFolder { get; private set; }
        public string? CurrentWallpaper { get; private set; }
        public List<string> Skips { get; private set; }

        public event Action? DateChange;

        public UserData()
        {
            Skips = new();
        }

        public UserData(int interval, int remainingTime, string? wallpaperFolder, string? currentWallpaper, List<string> skips)
        {
            Interval = interval;
            RemainingTime = remainingTime;
            WallpaperFolder = wallpaperFolder;
            CurrentWallpaper = currentWallpaper;
            Skips = skips;
        }

        public void SetInterval(int interval)
        {
            Interval = interval;
            DateChange?.Invoke();
        }

        public void SetRemainingTime(int remainingTime)
        {
            RemainingTime = remainingTime;
            DateChange?.Invoke();
        }

        public void SetWallpaperFolder(string wallpaperFolder)
        {
            WallpaperFolder = wallpaperFolder;
            DateChange?.Invoke();
        }

        public void SetCurrentWallpaper(string currentWallpaper)
        {
            CurrentWallpaper = currentWallpaper;
            DateChange?.Invoke();
        }

        public void AddSkip()
        {
            string fileName = Path.GetFileName(CurrentWallpaper)!;
            Skips.Add(fileName);
            DateChange?.Invoke();
        }

        public void RemoveSkip(string skip)
        {
            Skips.Remove(skip);
            DateChange?.Invoke();
        }
    }
}
