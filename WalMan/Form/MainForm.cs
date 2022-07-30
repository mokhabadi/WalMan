using WalMan.Properties;

namespace WalMan
{
    internal partial class MainForm : Form
    {
        public event Action<string>? WallpaperFolderChanged;
        public event Action<string>? IntervalChanged;

        static Settings Settings => Settings.Default;

        public MainForm()
        {
            InitializeComponent();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            intervalComboBox.Items.AddRange(Manager.TimeIntervals);

            foreach (string intervalName in Manager.TimeIntervals)
                if (Manager.timeIntervalDictionary[intervalName] == Settings.timerInterval)
                    intervalComboBox.SelectedValue = intervalName;

            wallpaperFolderLabel.Text = Settings.wallpaperFolder != "" ? Settings.wallpaperFolder : "not set";
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

        }
    }
}