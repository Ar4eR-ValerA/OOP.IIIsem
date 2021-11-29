using System;
using System.IO;
using System.Net.Sockets;
using Backups.Client.ServerStorages;
using Backups.Tools;

namespace Backups.Client.Tools
{
    public static class FileService
    {
        public static void SendFile(FileInfo localFileInfo, FileServerStorage fileServerStorage)
        {
            if (fileServerStorage is null)
            {
                throw new BackupsException("FileServerStorage is null");
            }

            if (localFileInfo is null)
            {
                throw new BackupsException("LocalFileInfo is null");
            }

            var tcpClient = new TcpClient(fileServerStorage.IpAddress, fileServerStorage.Port);
            byte[] bytes = File.ReadAllBytes(localFileInfo.FullName);

            var streamWriter = new StreamWriter(tcpClient.GetStream());

            streamWriter.WriteLine("send");
            streamWriter.Flush();

            streamWriter.WriteLine(bytes.Length.ToString());
            streamWriter.Flush();

            streamWriter.WriteLine(fileServerStorage.FilePath);
            streamWriter.Flush();

            tcpClient.Client.SendFile(localFileInfo.FullName);
            streamWriter.Flush();
            
            streamWriter.Close();
            tcpClient.Close();
        }

        public static void TakeFile(FileServerStorage fileServerStorage, string targetPath)
        {
            if (fileServerStorage is null)
            {
                throw new BackupsException("FileServerStorage is null");
            }

            if (targetPath is null)
            {
                throw new BackupsException("targetPath is null");
            }

            using var tcpClient = new TcpClient(fileServerStorage.IpAddress, fileServerStorage.Port);

            using var streamWriter = new StreamWriter(tcpClient.GetStream());

            streamWriter.WriteLine("take");
            streamWriter.Flush();

            streamWriter.WriteLine(fileServerStorage.FilePath);
            streamWriter.Flush();

            streamWriter.WriteLine(targetPath);
            streamWriter.Flush();

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
            tcpClient.Client.Close();
        }
    }
}