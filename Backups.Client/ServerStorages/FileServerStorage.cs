using System.Collections.Generic;
using System.IO;
using System.Net;
using Backups.Client.Interfaces;
using Backups.Tools;

namespace Backups.Client.ServerStorages
{
    public class FileServerStorage : IServerStorage
    {
        public FileServerStorage(FileInfo fileInfo, IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress ?? throw new BackupsException("Null argument");
            Port = port;
            FileInfo = fileInfo ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { FileInfo };
        public string Path => FileInfo.FullName;
        public FileInfo FileInfo { get; }

        public IPAddress IpAddress { get; }
        public int Port { get; }
    }
}