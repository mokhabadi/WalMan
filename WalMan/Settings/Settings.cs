namespace WalMan.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            PropertyChanged += (sender,eventArgs) => Save();
        }
    }
}