using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.JobObjects
{
    public class FilesJobObject : IJobObject
    {
        private readonly List<string> _filePaths;

        public FilesJobObject(List<string> filePaths)
        {
            _filePaths = filePaths ?? throw new BackupsException("Paths are null");
        }

        public FilesJobObject(string filePath)
        {
            _filePaths = new List<string> { filePath ?? throw new BackupsException("Path is null") };
        }

        [JsonConstructor]
        public FilesJobObject()
        {
            _filePaths = new List<string>();
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