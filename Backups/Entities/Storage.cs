using System.Collections.Generic;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities
{
    public class Storage
    {
        private readonly List<FileData> _files;

        public Storage()
        {
            _files = new List<FileData>();
        }

        public Storage(List<FileData> files)
        {
            _files = files ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileData> Files => _files;

        internal void AddFile(FileData fileData)
        {
            AddFiles(new List<FileData> { fileData ?? throw new BackupsException("Null argument") });
        }

        internal void AddFiles(IReadOnlyList<FileData> files)
        {
            _files.AddRange(files ?? throw new BackupsException("Null argument"));
        }
    }
}