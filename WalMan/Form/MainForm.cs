using System;
using System.Windows.Forms;

namespace WalMan
{
    internal partial class MainForm : Form
    {
        public static readonly int[] intervals = { 10, 30, 60, 180, 600, 1800, 3600, 7200, 10800 };

        public event Action<string?>? WallpaperChange;
        public event Action<int>? IntervalChanged;

        public MainForm()
        {
            InitializeComponent();

            foreach (int timeInterval in intervals)
                intervalComboBox.Items.Add(Statics.SecondToString(timeInterval));
        }

        public void Initialize(UserData userData)
        {
            currentWallpaperLabel.Text = userData.Wallpaper ?? "not set";

            for (int i = 0; i < intervals.Length; i++)
                if (userData.Interval == intervals[i])
                    intervalComboBox.SelectedIndex = i;

            skipListBox.Items.AddRange(userData.SkipList.ToArray());
        }

        void IntervalComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            IntervalChanged?.Invoke(intervals[intervalComboBox.SelectedIndex]);
        }

        void UnregisterButtonClick(object sender, EventArgs e)
        {
            WallpaperChange?.Invoke(null);
            Close();
        }

        void OpenLogButtonClick(object sender, EventArgs e)
        {
            Statics.OpenFile(Log.fileName);
        }

        void SelectWallpaperButtonClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentWallpaperLabel.Text = openFileDialog.FileName;
                WallpaperChange?.Invoke(openFileDialog.FileName);
            }
        }
    }
}