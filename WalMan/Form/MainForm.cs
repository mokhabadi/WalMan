using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WalMan
{
    internal partial class MainForm : Form
    {
        public event Action<string>? WallpaperFolderChanged;
        public event Action<int>? IntervalChanged;
        public event Action? DisableClicked;
        public static readonly int[] intervals = { 10, 30, 60, 180, 600, 1800, 3600, 7200, 10800 };

        public MainForm()
        {
            InitializeComponent();

            foreach (int timeInterval in intervals)
                intervalComboBox.Items.Add(Manager.SecondToString(timeInterval));
        }

        public void Initialize(string wallpaperFolder, int currentInterval, string[] skips)
        {
            wallpaperFolderLabel.Text = wallpaperFolder != "" ? wallpaperFolder : "not set";

            for (int i = 0; i < intervals.Length; i++)
                if (currentInterval == intervals[i])
                    intervalComboBox.SelectedIndex = i;

            skipListBox.Items.AddRange(skips);
        }

        void SelectFolderButtonClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                wallpaperFolderLabel.Text = folderBrowserDialog.SelectedPath;
                WallpaperFolderChanged?.Invoke(folderBrowserDialog.SelectedPath);
            }
        }

        void IntervalComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            IntervalChanged?.Invoke(intervals[intervalComboBox.SelectedIndex]);
        }

        void UnregisterButtonClick(object sender, EventArgs e)
        {
            DisableClicked?.Invoke();
        }

        void OpenLogButtonClick(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Log.fileName);
        }
    }
}