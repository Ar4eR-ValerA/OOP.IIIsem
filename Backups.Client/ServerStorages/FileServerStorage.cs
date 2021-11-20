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
            IpAddress = ipAddress ?? throw new BackupsException("IpAddress is null");
            Port = port;
            FileInfo = fileInfo ?? throw new BackupsException("FileInfo is null");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { FileInfo };

        public string Path
        {
            get => FileInfo.FullName;
            set => FileInfo = new FileInfo(value);
        }

        public FileInfo FileInfo { get; set; }

        public IPAddress IpAddress { get; }
        public int Port { get; }
    }
}