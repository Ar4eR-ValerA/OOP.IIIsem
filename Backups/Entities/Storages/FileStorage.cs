using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities.Storages
{
    public class FileStorage : IStorage
    {
        [JsonIgnore]
        private string _filePath;

        [JsonConstructor]
        public FileStorage(string filePath)
        {
            _filePath = filePath ?? throw new BackupsException("File path is null");
        }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths => new List<string> { _filePath };

        [JsonProperty]
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
            private set => _filePath = value ?? throw new BackupsException("value is null");
        }
    }
}