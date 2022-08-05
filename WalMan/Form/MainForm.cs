using System.Diagnostics;

namespace WalMan
{
    internal partial class MainForm : Form
    {
        public event Action<string>? WallpaperFolderChanged;
        public event Action<int>? IntervalChanged;
        public event Action? Loaded;
        public event Action? DisableClicked;

        public MainForm()
        {
            InitializeComponent();
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            if (intervalComboBox.Items.Count == 0)
                foreach (int timeInterval in Manager.timeIntervals)
                    intervalComboBox.Items.Add(Manager.SecondToString(timeInterval));

            Loaded?.Invoke();
        }

        public void Initialize(string wallpaperFolder, int currentInterval, string[] skips)
        {
            wallpaperFolderLabel.Text = wallpaperFolder != "" ? wallpaperFolder : "not set";
            intervalComboBox.SelectedIndex = currentInterval;
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
            IntervalChanged?.Invoke(intervalComboBox.SelectedIndex);
        }

        void UnregisterButtonClick(object sender, EventArgs e)
        {
            DisableClicked?.Invoke();
        }

        void OpenLogButtonClick(object sender, EventArgs e)
        {
            string filePath = Application.StartupPath + "Log.txt";
            Process.Start("notepad.exe", filePath);
        }
    }
}