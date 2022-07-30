using Microsoft.Win32;

namespace WalMan
{
    internal static class WindowsRegistry
    {
        static readonly string applicationName = Application.ProductName;

        public static void EnableFeatures()
        {
            CreateDesktopMenu();
            StartUp(true);
        }

        public static void DisableFeatures()
        {
            DeleteDesktopMenu();
            StartUp(false);
        }

        static void CreateDesktopMenu()
        {
            string desktopShell = @"SOFTWARE\Classes\Directory\Background\shell";
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(desktopShell, true);

            if (key == null)
                return;

            if (key.OpenSubKey(applicationName) != null)
                return;

            key = key.CreateSubKey(applicationName);
            key.SetValue("SubCommands", "");
            key.SetValue("Icon", @"%WinDir%\system32\imageres.dll,-110", RegistryValueKind.ExpandString);
            RegistryKey rootRegistryKey = key.CreateSubKey("shell");
            int index = 1;

            foreach (Command command in Manager.commandList)
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
