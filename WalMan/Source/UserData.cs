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
        List<string> skips;

        public string[] Skips => skips.ToArray();

        public event Action? DateChange;

        public UserData()
        {
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
            skips.Add(fileName);
            DateChange?.Invoke();
        }

        public void RemoveSkip(string skip)
        {
            skips.Remove(skip);
            DateChange?.Invoke();
        }

        public bool SkipsContain(string fileName)
        {
            return skips.Contains(fileName);
        }

        public void Reset()
        {
            WallpaperFolder = null;
            CurrentWallpaper = null;
            RemainingTime = 0;
        }
    }
}
