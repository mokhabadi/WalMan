namespace WalMan.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            PropertyChanged += (sender,eventArgs) => Save();
            PropertyChanged += (sender,eventArgs) => Log.Add($"Property {eventArgs.PropertyName} saved");            
        }
    }
}