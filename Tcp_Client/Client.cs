using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tcp_Client
{
    class Client
    {
        class TestSocket
        {
            Socket socketCln = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] buffer = new byte[1024];

            ~TestSocket()
            {
                socketCln.Shutdown(SocketShutdown.Both);
                socketCln.Close();
            }

            public void StartSocket()
            {
                socketCln.Connect(IPAddress.Parse("192.168.1.149"), 50);

                while (true)
                {
                    string message = Console.ReadLine();
                    buffer = Encoding.UTF8.GetBytes(message);

                    socketCln.Send(buffer);
                    buffer = new byte[1024];
                    int bytes = socketCln.Receive(buffer);
                    Console.WriteLine((Encoding.UTF8.GetString(buffer, 0, bytes)));
                }
            }
        }

        class TestTcpClient
        {
            TcpClient tcpClient = new TcpClient();
            NetworkStream stream;

            ~TestTcpClient()
            {
                stream.Close();
                tcpClient.Close();
            }

            public void StartTcpclient()
            {
                tcpClient.Connect(IPAddress.Parse("192.168.1.149"), 50);
                stream = tcpClient.GetStream();

                Task task = Task.Run(() =>
                 {
                     byte[] buffer;
                     string message;
                     Thread.CurrentThread.IsBackground = false;

                     while (true)
                     {
                         message = Console.ReadLine();

                         buffer = Encoding.UTF8.GetBytes(message);
                         stream.Write(buffer, 0, buffer.Length);
                     }
                 });


                Task.Run(() =>
                {
                    byte[] buffer;
                    string message;
                    Thread.CurrentThread.IsBackground = false;

                    while (true)
                    {
                        buffer = new byte[1024];
                        int bytes = stream.Read(buffer, 0, buffer.Length);

                        message = Encoding.UTF8.GetString(buffer, 0, bytes);
                        Console.WriteLine(message);
                    }
                });
            }
        }


        static void Main(string[] args)
        {
            new TestSocket().StartSocket();
            //new TestTcpClient().StartTcpclient();

            Console.ReadLine();
        }
    }
}
