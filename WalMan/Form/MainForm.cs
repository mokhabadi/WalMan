using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WalMan
{
    internal partial class MainForm : Form
    {
        public static readonly int[] intervals = { 10, 30, 60, 180, 600, 1800, 3600, 7200, 10800 };

        public event Action<string?>? CurrentWallpaperChange;
        public event Action<int>? IntervalChanged;

        public MainForm()
        {
            InitializeComponent();

            foreach (int timeInterval in intervals)
                intervalComboBox.Items.Add(Statics.SecondToString(timeInterval));
        }

        public void Initialize(string? currentWallpaper, int currentInterval, string[] skips)
        {
            currentWallpaperLabel.Text = currentWallpaper ?? "not set";

            for (int i = 0; i < intervals.Length; i++)
                if (currentInterval == intervals[i])
                    intervalComboBox.SelectedIndex = i;

            skipListBox.Items.AddRange(skips);
        }

        void IntervalComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            IntervalChanged?.Invoke(intervals[intervalComboBox.SelectedIndex]);
        }

        void UnregisterButtonClick(object sender, EventArgs e)
        {
            CurrentWallpaperChange?.Invoke(null);
            Close();
        }

        void OpenLogButtonClick(object sender, EventArgs e)
        {
            Process.Start(Log.fileName);
        }

        void SelectFolderButtonClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentWallpaperLabel.Text = openFileDialog.FileName;
                CurrentWallpaperChange?.Invoke(openFileDialog.FileName);
            }
        }
    }
}