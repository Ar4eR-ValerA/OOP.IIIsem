using System.IO;
using System.Net;
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

            streamWriter.Close();
            tcpClient.Close();
        }

        public static void TakeFile(FileServerStorage fileServerStorage, string targetPath, int transferPort)
        {
            if (fileServerStorage is null)
            {
                throw new BackupsException("FileServerStorage is null");
            }

            if (targetPath is null)
            {
                throw new BackupsException("targetPath is null");
            }

            var tcpClient = new TcpClient(fileServerStorage.IpAddress, fileServerStorage.Port);

            var streamWriter = new StreamWriter(tcpClient.GetStream());

            streamWriter.WriteLine("take");
            streamWriter.Flush();

            streamWriter.WriteLine(fileServerStorage.FilePath);
            streamWriter.Flush();

            streamWriter.WriteLine(targetPath);
            streamWriter.Flush();

            streamWriter.WriteLine(transferPort.ToString());
            streamWriter.Flush();

            streamWriter.Close();
            tcpClient.Close();

            Receiver.ReceiveFile(IPAddress.Parse(fileServerStorage.IpAddress), transferPort);
        }
    }
}