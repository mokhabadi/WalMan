using Microsoft.Win32;
using System.Windows.Forms;

namespace WalMan
{
    internal static class WindowsRegistry
    {
        static readonly string applicationName = Application.ProductName;
        static readonly string startUpRegisteryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        static readonly string desktopShellRegisteryPath = @"SOFTWARE\Classes\Directory\Background\shell";

        public static void EnableFeatures(Command[] commands)
        {
            CreateDesktopMenu(commands);
            StartUp(true);
        }

        public static void DisableFeatures()
        {
            DisableDesktopMenu();
            StartUp(false);
        }

        static void StartUp(bool enable)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(startUpRegisteryPath, true);

            if (enable)
                key?.SetValue(applicationName, Application.ExecutablePath.Enquote());
            else
                key?.DeleteValue(applicationName, false);
        }

        static void CreateDesktopMenu(Command[] commands)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(desktopShellRegisteryPath, true);

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

        static void DisableDesktopMenu()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(desktopShellRegisteryPath, true);

            if (key?.OpenSubKey(applicationName) != null)
                key.DeleteSubKeyTree(applicationName);
        }
    }
}