using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinRemoteDesktop
{
    static class Program
    {
        private static Mutex mutex = new Mutex(true, "MyApp");

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
                mutex.ReleaseMutex();
            }
            else {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut))
                {
                    Console.WriteLine("NamedPipeClientStream object created.");

                    // Connect to the server
                    Console.Write("Connecting to server...");
                    pipeClient.Connect();
                    Console.WriteLine("Connected to server.");

                    // Send a request to the server
                    string request = "Hello from the client!";
                    byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                    pipeClient.Write(requestBytes, 0, requestBytes.Length);
                    Console.WriteLine("Request sent: {0}", request);

                    //// Read the response from the server
                    //byte[] buffer = new byte[1024];
                    //int bytesRead = pipeClient.Read(buffer, 0, 1024);
                    //string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //Console.WriteLine("Received response: {0}", response);
                }
            }
        }
    }
}
