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
        private string _directoryPath;
        private List<string> _filePaths;

        [JsonConstructor]
        public DirectoryStorage()
        {
        }

        public DirectoryStorage(string directoryPath)
        {
            _directoryPath = directoryPath ?? throw new BackupsException("Path is null");
            _filePaths = Directory.GetFiles(_directoryPath).ToList();
        }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths
        {
            get => _filePaths;
            private set => _filePaths = value.ToList();
        }

        public string Path
        {
            get => _directoryPath;
            private set => _directoryPath = value ?? throw new BackupsException("value is null");
        }
    }
}