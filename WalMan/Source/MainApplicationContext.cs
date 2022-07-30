using System.Reflection;
using WalMan.Properties;

namespace WalMan
{
    internal class MainApplicationContext : ApplicationContext
    {
        readonly NotifyIcon notifyIcon;

        public MainApplicationContext()
        {
            List<ToolStripItem> toolStripItems = new();

            foreach (Command command in Manager.commandList)
                toolStripItems.Add(new ToolStripMenuItem(command.Description, null, (sender, eventArgs) => command.Action()));

            toolStripItems.Add(new ToolStripSeparator());
            toolStripItems.Add(new ToolStripMenuItem("Open", null, (sender, eventArgs) => Manager.Open()));
            toolStripItems.Add(new ToolStripMenuItem("Exit", null, (sender, eventArgs) => Application.Exit()));
            ContextMenuStrip contextMenuStrip = new();
            contextMenuStrip.Items.AddRange(toolStripItems.ToArray());

            notifyIcon = new NotifyIcon()
            {
                Icon = Resources.notifyIcon,
                ContextMenuStrip = contextMenuStrip,
                Visible = true,
                Text = "Disable",
            };

            notifyIcon.MouseMove += NotifyIconBalloonTipShown;
            notifyIcon.MouseUp += NotifyIconMouseUp;
            Application.ApplicationExit += (sender, eventArgs) => notifyIcon.Visible = false;
            Manager.Load();
        }

        async void NotifyIconBalloonTipShown(object? sender, EventArgs e)
        {
            notifyIcon.MouseMove -= NotifyIconBalloonTipShown;
            await Task.Delay(500);
            notifyIcon.MouseMove += NotifyIconBalloonTipShown;
        }

        void NotifyIconMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            MethodInfo? methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo?.Invoke(notifyIcon, null);
        }
    }
}