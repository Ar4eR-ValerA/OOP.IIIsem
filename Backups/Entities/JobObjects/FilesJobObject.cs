using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities.JobObjects
{
    public class FilesJobObject : IJobObject
    {
        [JsonProperty("filePaths")]
        private readonly List<string> _filePaths;

        [JsonConstructor]
        public FilesJobObject(List<string> filePaths)
        {
            _filePaths = filePaths ?? throw new BackupsException("Paths are null");
        }

        public FilesJobObject(string filePath)
        {
            _filePaths = new List<string> { filePath ?? throw new BackupsException("Path is null") };
        }

        [JsonIgnore]
        public IReadOnlyList<string> FilePaths => _filePaths;

        public void AddFile(string filePath)
        {
            _filePaths.Add(filePath ?? throw new BackupsException("Path is null"));
        }

        public void RemoveFile(string filePath)
        {
            _filePaths.Remove(filePath ?? throw new BackupsException("Path is null"));
        }
    }
}