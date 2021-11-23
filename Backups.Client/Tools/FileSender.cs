using System.IO;
using System.Net.Sockets;
using Backups.Client.ServerStorages;
using Backups.Tools;

namespace Backups.Client.Tools
{
    public static class FileSender
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
            streamWriter.WriteLine(bytes.Length.ToString());
            streamWriter.Flush();

            streamWriter.WriteLine(fileServerStorage.FilePath);
            streamWriter.Flush();

            tcpClient.Client.SendFile(localFileInfo.FullName);

            streamWriter.Close();
            tcpClient.Close();
        }
    }
}