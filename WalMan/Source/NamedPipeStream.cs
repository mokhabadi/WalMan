using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WalMan
{
    internal static class NamedPipeStream
    {
        public static event Action<string>? OnReceive;

        public static readonly string PipeName = Application.ProductName;

        public static async Task Send(string command)
        {
            NamedPipeClientStream namedPipeClientStream = new(".", PipeName, PipeDirection.Out);
            await namedPipeClientStream.ConnectAsync(1000);
            byte[] message = Encoding.Default.GetBytes(command);
            await namedPipeClientStream.WriteAsync(message);
            namedPipeClientStream.Close();
        }

        public static async void Receive()
        {
            NamedPipeServerStream namedPipeServerStream = new(PipeName, PipeDirection.In, 1);
            byte[] buffer = new byte[1024];

            while (true)
            {
                await namedPipeServerStream.WaitForConnectionAsync();
                int length = await namedPipeServerStream.ReadAsync(buffer.AsMemory(0, 1024));
                string message = Encoding.Default.GetString(buffer, 0, length);
                OnReceive?.Invoke(message);
                namedPipeServerStream.Disconnect();
            }
        }
    }
}
