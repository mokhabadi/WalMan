using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WalMan.Properties;

namespace WalMan
{
    internal class MainApplicationContext : ApplicationContext
    {
        readonly NotifyIcon notifyIcon;
        readonly Manager manager;

        public MainApplicationContext()
        {
            Log.Add(@"//////////////// Application Run \\\\\\\\\\\\\\\\");
            List<ToolStripItem> toolStripItems = new();
            manager = new();

            foreach (Command command in manager.commands)
                toolStripItems.Add(new ToolStripMenuItem(command.Description, null, (sender, eventArgs) => command.Action()));

            toolStripItems.Add(new ToolStripSeparator());
            toolStripItems.Add(new ToolStripMenuItem("Open", null, (sender, eventArgs) => manager.Open()));
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
            manager.Load();
        }

        async void NotifyIconMouseMove(object? sender, EventArgs e)
        {
            notifyIcon.MouseMove -= NotifyIconMouseMove;
            notifyIcon.Text = manager.GetRemaining();
            await Task.Delay(1000);
            notifyIcon.MouseMove += NotifyIconMouseMove;
        }

        void NotifyIconMouseUp(object? sender, MouseEventArgs MouseEventArgs)
        {
            if (MouseEventArgs.Button != MouseButtons.Left)
                return;

            MethodInfo? methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo?.Invoke(notifyIcon, null);
        }
    }
}