using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WalMan
{
    internal static class Program
    {
        static Mutex? mutex;

        [STAThread]
        static void Main()
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            mutex = new Mutex(false, Application.ProductName);

            if (mutex.WaitOne(0))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainApplicationContext());
                mutex.ReleaseMutex();
            }

            string[] parameters = Environment.GetCommandLineArgs();

            if (parameters.Length >= 2)
            {
                string command = parameters[1];
                await NamedPipeStream.Send(command);
            }
        }
    }
}