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

            foreach (Command command in Manager.Commands)
                toolStripItems.Add(new ToolStripMenuItem(command.Description, null, (sender, eventArgs) => command.Action()));

            toolStripItems.Add(new ToolStripSeparator());
            toolStripItems.Add(new ToolStripMenuItem("Open", null, (sender, eventArgs) => Manager.Open()));
            toolStripItems.Add(new ToolStripMenuItem("Exit", null, (sender, eventArgs) => Application.Exit()));
            ContextMenuStrip contextMenuStrip = new();
            contextMenuStrip.Items.AddRange(toolStripItems.ToArray());

            notifyIcon = new NotifyIcon()
            {
                Icon = Resources.WalMan,
                ContextMenuStrip = contextMenuStrip,
                Visible = true,
                Text = "Disable",
            };

            notifyIcon.MouseMove += NotifyIconMouseMove;
            notifyIcon.MouseUp += NotifyIconMouseUp;
            Application.ApplicationExit += (sender, eventArgs) => notifyIcon.Visible = false;
            Manager.Load();
        }

        async void NotifyIconMouseMove(object? sender, EventArgs e)
        {
            notifyIcon.MouseMove -= NotifyIconMouseMove;
            notifyIcon.Text = Manager.GetRemaining();
            await Task.Delay(500);
            notifyIcon.MouseMove += NotifyIconMouseMove;
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