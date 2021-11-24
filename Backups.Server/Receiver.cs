using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Backups.Client.ServerStorages;
using Backups.Tools;

namespace Backups.Server
{
    public static class Receiver
    {
        public static void Receive(IPAddress ipAddress, int port)
        {
            var tcpListener = new TcpListener(
                ipAddress ?? throw new BackupsException("IpAddress is null"),
                port);

            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                var streamReader = new StreamReader(tcpClient.GetStream());
                string mode = streamReader.ReadLine();

                if (mode == "send")
                {
                    string fileSize = streamReader.ReadLine();
                    string fileName = streamReader.ReadLine();

                    int length = Convert.ToInt32(fileSize);
                    byte[] buffer = new byte[length];

                    tcpClient.GetStream().Read(buffer, 0, length);

                    using var fileStream = new FileStream(fileName ?? string.Empty, FileMode.Create);
                    fileStream.Write(buffer, 0, buffer.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    Thread.Sleep(1000);
                    tcpClient.Close();
                    tcpListener.Stop();

                    return;
                }

                if (mode == "take")
                {
                    string fileName = streamReader.ReadLine();
                    string targetFile = streamReader.ReadLine();
                    int transferPort = Convert.ToInt32(streamReader.ReadLine());
                    
                    Thread.Sleep(1000);
                    tcpClient.Close();
                    tcpListener.Stop();
                    FileSender.SendFile(
                        fileName,
                        new FileServerStorage(targetFile, ipAddress.ToString(), transferPort));

                    return;
                }
            }
        }
    }
}