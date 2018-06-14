using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{
    class Server
    {
        class TestSocket
        {
             Socket socketSrv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            

            public void StartSocket()
            {
                socketSrv.Bind(new IPEndPoint(IPAddress.Any, 50));
                socketSrv.Listen(5);

                while (true)
                {
                    Socket socketClient = socketSrv.Accept(); // Блокирует вызывающий поток до появления нового подключения

                    Console.WriteLine("New conection");

                    Task.Run(() =>
                   {
                       for (; ; )
                       {
                           byte[] buffer = new byte[1024];

                           int bytes = socketClient.Receive(buffer);
                           Console.WriteLine((Encoding.UTF8.GetString(buffer, 0, bytes)));
                           buffer = Encoding.ASCII.GetBytes("Message received");
                           socketClient.Send(buffer);
                       }
                   });
                }

                socketSrv.Shutdown(SocketShutdown.Both);
                socketSrv.Close();
            }
        }

        class TestTcpListener
        {
            static TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, 50));
            NetworkStream stream;

            public void StartTcpListener()
            {
                tcpListener.Start();

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient(); //Socket socket = tcpListener.AcceptSocket();

                    Console.WriteLine("New conection");

                    Task.Run(() =>
                    {
                        stream = tcpClient.GetStream();
                        for (; ; )
                        {
                            byte[] buffer = new byte[1024];

                            int bytes = stream.Read(buffer, 0, buffer.Length);
                            Console.WriteLine((Encoding.UTF8.GetString(buffer, 0, bytes)));

                            buffer = Encoding.ASCII.GetBytes("Message received");
                            stream.Write(buffer, 0, buffer.Length);
                        }
                        stream.Close();
                        tcpClient.Close();
                    });
                }
            }
        }

        static void Main(string[] args)
        {
         //new TestSocket().StartSocket();
           new TestTcpListener().StartTcpListener();

        }
    }
}
