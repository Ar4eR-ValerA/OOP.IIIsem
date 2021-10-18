using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Backups.Server
{
    public static class Receiver
    {
        public static void ReceiveFile(int port)
        {
            var tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();  

                var streamReader = new StreamReader(tcpClient.GetStream());
                string fileSize = streamReader.ReadLine();
                string fileName = streamReader.ReadLine();

                int length = Convert.ToInt32(fileSize);
                byte[] buffer = new byte[length];

                tcpClient.GetStream().Read(buffer, 0, length);

                using var fileStream = new FileStream(fileName ?? string.Empty, FileMode.Create);
                fileStream.Write(buffer, 0, buffer.Length);  
                fileStream.Flush();  
                fileStream.Close();
            }
        }
    }
}