namespace WalMan.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            PropertyChanged += (sender,eventArgs) => Save();
            PropertyChanged += (sender,eventArgs) => Log.Add($"Settings saved: {eventArgs.PropertyName} = {this[eventArgs.PropertyName]}");            
        }
    }
}