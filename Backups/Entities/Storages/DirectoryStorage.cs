using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities.Storages
{
    public class DirectoryStorage : IStorage
    {
        [JsonConstructor]
        public DirectoryStorage(string directoryPath)
        {
            DirectoryPath = directoryPath ?? throw new BackupsException("Path is null");
        }

        [JsonProperty]
        public string DirectoryPath { get; private set; }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths => Directory.GetFiles(DirectoryPath);

        public string Path
        {
            get => DirectoryPath;
            private set => DirectoryPath = value ?? throw new BackupsException("value is null");
        }
    }
}