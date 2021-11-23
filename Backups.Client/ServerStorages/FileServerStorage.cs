using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backups.Client.Interfaces;
using Backups.Tools;

namespace Backups.Client.ServerStorages
{
    public class FileServerStorage : IServerStorage
    {
        public FileServerStorage(string fileInfo, string ipAddress, int port)
        {
            IpAddress = ipAddress ?? throw new BackupsException("IpAddress is null");
            Port = port;
            FilePath = fileInfo ?? throw new BackupsException("FilePath is null");
        }
        
        [JsonConstructor]
        public FileServerStorage()
        {
        }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths => new List<string> { FilePath };

        public string Path
        {
            get => FilePath;
            set => FilePath = value ?? throw new BackupsException("File Path is null");
        }

        public string FilePath { get; private set; }
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
    }
}