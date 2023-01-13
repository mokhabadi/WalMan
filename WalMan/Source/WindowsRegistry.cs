using Microsoft.Win32;
using System.Windows.Forms;

namespace WalMan
{
    internal static class WindowsRegistry
    {
        static readonly string applicationName = Application.ProductName;

        public static void EnableFeatures(Command[] commands)
        {
            CreateDesktopMenu(commands);
            StartUp(true);
        }

        public static void DisableFeatures()
        {
            DeleteDesktopMenu();
            StartUp(false);
        }

        static void CreateDesktopMenu(Command[] commands)
        {
            string desktopShell = @"SOFTWARE\Classes\Directory\Background\shell";
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(desktopShell, true);

            if (key == null)
                return;

            if (key.OpenSubKey(applicationName) != null)
                return;

            key = key.CreateSubKey(applicationName);
            key.SetValue("SubCommands", "");
            key.SetValue("Icon", Application.ExecutablePath.Enquote(), RegistryValueKind.ExpandString);
            RegistryKey rootRegistryKey = key.CreateSubKey("shell");
            int index = 1;

            foreach (Command command in commands)
            {
                key = rootRegistryKey.CreateSubKey(index.ToString() + "- " + command);
                key.SetValue(null, command.Description);
                //key.SetValue("Icon", menuItems[index].iconPath);
                key = key.CreateSubKey("command");
                key.SetValue(null, Application.ExecutablePath.Enquote() + command, RegistryValueKind.ExpandString);
                index++;
            }
        }

        static void DeleteDesktopMenu()
        {
            string desktopShell = @"SOFTWARE\Classes\Directory\Background\shell";
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(desktopShell, true);

            if (key?.OpenSubKey(applicationName) != null)
                key.DeleteSubKeyTree(applicationName);
        }

        static void StartUp(bool enable)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (enable)
                key?.SetValue(applicationName, Application.ExecutablePath.Enquote());
            else if (key?.GetValue(applicationName) != null)
                key.DeleteValue(applicationName);
        }

        public static string Enquote(this string value)
        {
            return $"\"{value}\"";
        }
    }
}