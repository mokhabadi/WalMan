using System;
using System.Collections.Generic;
using System.IO;

namespace WalMan
{
    internal class UserData
    {
        public int Interval { get; private set; }
        public int RemainingTime { get; private set; }
        public string? CurrentWallpaper { get; private set; }

        readonly List<string> skips;

        public string[] Skips => skips.ToArray();

        public event Action? DateChange;
        public event Action? CurrentWallpaperChange;

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

        public void SetCurrentWallpaper(string? currentWallpaper)
        {
            CurrentWallpaper = currentWallpaper;
            CurrentWallpaperChange?.Invoke();
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
            CurrentWallpaper = null;
            RemainingTime = 0;
        }
    }
}
