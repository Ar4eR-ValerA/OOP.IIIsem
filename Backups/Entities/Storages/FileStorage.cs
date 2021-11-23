using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class FileStorage : IStorage
    {
        private string _filePath;

        [JsonConstructor]
        public FileStorage(string filePath)
        {
            _filePath = filePath ?? throw new BackupsException("File path is null");
        }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths => new List<string> { _filePath };

        public string Path
        {
            get
            {
                if (_filePath is null)
                {
                    throw new BackupsException("There is no file path");
                }

                return _filePath;
            }
            set => _filePath = value ?? throw new BackupsException("value is null");
        }
    }
}