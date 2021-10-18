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
            if (localFileInfo is null || fileServerStorage is null)
            {
                throw new BackupsException("Null argument");
            }
            var tcpClient = new TcpClient(fileServerStorage.IpAddress.ToString(), fileServerStorage.Port);
            byte[] bytes = File.ReadAllBytes(localFileInfo.FullName);

            var streamWriter = new StreamWriter(tcpClient.GetStream());
            streamWriter.WriteLine(bytes.Length.ToString());
            streamWriter.Flush();

            streamWriter.WriteLine(fileServerStorage.FileInfo.FullName);
            streamWriter.Flush();

            tcpClient.Client.SendFile(localFileInfo.FullName);
            
            streamWriter.Close();
            tcpClient.Close();
        }
    }
}