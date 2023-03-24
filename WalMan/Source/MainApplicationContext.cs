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
        static readonly NotifyIcon notifyIcon;
        readonly Manager manager;

        static MainApplicationContext()
        {
            notifyIcon = new NotifyIcon()
            {
                Icon = Resources.WalMan,
                Visible = true,
                Text = "Disable",
            };
        }

        public MainApplicationContext()
        {
            Log.Add(@"//////////////// Application Run \\\\\\\\\\\\\\\\");
            manager = new();
            notifyIcon.MouseMove += NotifyIconMouseMove;
            notifyIcon.MouseUp += NotifyIconMouseUp;
            Application.ApplicationExit += (sender, eventArgs) => notifyIcon.Visible = false;
            manager.Initialize();
        }

        public static void CreateMenu(Command[] commands)
        {
            List<ToolStripItem> toolStripItems = new();

            foreach (Command command in commands)
                toolStripItems.Add(new ToolStripMenuItem(command.Name, null, (sender, eventArgs) => command.Action()));

            ContextMenuStrip contextMenuStrip = new();
            contextMenuStrip.Items.AddRange(toolStripItems.ToArray());
            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        async void NotifyIconMouseMove(object? sender, EventArgs e)
        {
            notifyIcon.MouseMove -= NotifyIconMouseMove;
            int remainingTime = manager.GetRemainingTime();
            notifyIcon.Text = remainingTime > 0 ? Statics.SecondToString(remainingTime) : "Disabled";
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