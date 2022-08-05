namespace WalMan
{
    internal partial class MainForm : Form
    {
        public event Action<string>? WallpaperFolderChanged;
        public event Action<string>? IntervalChanged;
        public event Action? Loaded;
        public event Action? DisableClicked;

        public MainForm()
        {
            InitializeComponent();
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            intervalComboBox.Items.AddRange(Manager.TimeIntervals);
            Loaded?.Invoke();
        }

        public void Initialize(string wallpaperFolder,string? currentInterval, string[] skips)
        {
            wallpaperFolderLabel.Text = wallpaperFolder != "" ? wallpaperFolder : "not set";
            intervalComboBox.Text = currentInterval;
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
            string selectedInterval = (string)intervalComboBox.SelectedItem;
            IntervalChanged?.Invoke(selectedInterval);
        }

        void UnregisterButtonClick(object sender, EventArgs e)
        {
            DisableClicked?.Invoke();
        }
    }
}