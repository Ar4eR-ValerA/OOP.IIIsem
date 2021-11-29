using System.Collections.Generic;
using Backups.Client.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

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
            private set => FilePath = value ?? throw new BackupsException("File Path is null");
        }

        [JsonProperty]
        public string FilePath { get; private set; }

        [JsonProperty]
        public string IpAddress { get; private set; }

        [JsonProperty]
        public int Port { get; private set; }
    }
}