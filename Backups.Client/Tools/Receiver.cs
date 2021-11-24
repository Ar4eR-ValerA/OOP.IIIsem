using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Backups.Tools;

namespace Backups.Client.Tools
{
    public static class Receiver
    {
        public static void ReceiveFile(IPAddress ipAddress, int port)
        {
            var tcpListener = new TcpListener(
                ipAddress ?? throw new BackupsException("IpAddress is null"),
                port);

            tcpListener.Start();

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