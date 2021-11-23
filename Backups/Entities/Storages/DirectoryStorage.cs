using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class DirectoryStorage : IStorage
    {
        private List<string> _filePaths;

        [JsonConstructor]
        public DirectoryStorage(string directoryPath)
        {
            DirectoryPath = directoryPath ?? throw new BackupsException("Path is null");
            _filePaths = Directory.GetFiles(DirectoryPath).ToList();
        }

        public string DirectoryPath { get; private set; }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths
        {
            get => _filePaths;
            private set => _filePaths = value.ToList();
        }

        public string Path
        {
            get => DirectoryPath;
            private set => DirectoryPath = value ?? throw new BackupsException("value is null");
        }
    }
}