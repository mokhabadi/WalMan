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
            else
            {
                string[] parameters = Environment.GetCommandLineArgs();
                string command = parameters.Length >= 2 ? parameters[1] : "Open";
                await NamedPipeStream.Send(command);
            }
        }
    }
}