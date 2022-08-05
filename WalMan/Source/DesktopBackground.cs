using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WalMan
{
    internal static class DesktopBackground
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style
        {
            Center,
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
        }

        static readonly Dictionary<Style, (int Style, int Tile)> styles = new()
        {
            {Style.Center, (0,0)},
            {Style.Fill, (10,0)},
            {Style.Fit, (6,0)},
            {Style.Span, (22,0)},
            {Style.Stretch, (2,0)},
            {Style.Tile, (0,1)},
        };

        public static async Task Set(string filePath, Style style)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            if (key == null)
                return;

            key.SetValue(@"WallpaperStyle", styles[style].Style.ToString());
            key.SetValue(@"TileWallpaper", styles[style].Tile.ToString());
            int result = await Task.Run(() => SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE));
            Log.Add("Wallpaper Set " + (result > 0 ? "Successfully" : "failed"));
        }
    }
}
