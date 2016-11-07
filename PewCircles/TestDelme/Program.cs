using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDelme
{
    class Program
    {
        ASCIIEncoding asen = new ASCIIEncoding();

        private void Start()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 12345);
            tcpListener.Start();
            var socket = tcpListener.AcceptSocket();

            while (true)
            {
                socket.Send(asen.GetBytes("tralala"));
                Thread.Sleep(1000);
            }

            //TcpClient tcpClient = tcpListener.AcceptTcpClient();
            //NetworkStream clientStream = tcpClient.GetStream();
            //var writer = new StreamWriter(clientStream);
            //while (true)
            //{
            //    Console.WriteLine("Server is writing.");
            //    writer.WriteLine("X?");
            //    Thread.Sleep(1000);
            //}
        }

        private void Client()
        {
            TcpClient client = new TcpClient("localhost", 12345);
            NetworkStream _serverStream = client.GetStream();

            while (true)
            {
                byte[] bb = new byte[100];
                int x = _serverStream.Read(bb, 0, 100);
                for (int i = 0; i < x; i++)
                {
                    Console.WriteLine(Convert.ToChar(bb[i]));
                }
            }

            //var reader = new StreamReader(_serverStream);
            //while (true)
            //{
            //    Console.WriteLine("Client reading...");
            //    var x = reader.Read();
            //    Console.WriteLine("Client read!");
            //    Console.WriteLine(x);
            //}
        }

        static void Main(string[] args)
        {
            var p = new Program();

            Task.Run(() => { p.Start(); });

            p.Client();
        }
    }
}
