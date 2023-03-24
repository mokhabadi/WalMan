using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WalMan
{
    internal class UserData
    {
        [JsonInclude] public int Interval { get; private set; }
        [JsonInclude] public int RemainingTime { get; private set; }
        [JsonInclude] public string? Wallpaper { get; private set; }
        [JsonInclude] public List<string> SkipList { get; private set; }

        public event Action? DateChange;
        public event Action? WallpaperChange;

        public UserData()
        {
            SkipList = new();
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

        public void SetWallpaper(string? wallpaper)
        {
            Wallpaper = wallpaper;
            WallpaperChange?.Invoke();
            DateChange?.Invoke();
        }

        public void AddSkip()
        {
            SkipList.Add(Wallpaper);
            DateChange?.Invoke();
        }

        public void RemoveSkip(string skip)
        {
            SkipList.Remove(skip);
            DateChange?.Invoke();
        }

        public bool SkipsContain(string fileName)
        {
            return SkipList.Contains(fileName);
        }

        public void Reset()
        {
            Wallpaper = null;
            RemainingTime = 0;
        }
    }
}
