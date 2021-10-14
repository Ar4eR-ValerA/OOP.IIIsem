using System.Collections.Generic;
using System.IO;
using Backups.Tools;

namespace Backups.Entities
{
    public class Storage
    {
        private readonly List<FileInfo> _files;

        public Storage()
        {
            _files = new List<FileInfo>();
        }

        public Storage(List<FileInfo> files)
        {
            _files = files ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> Files => _files;

        internal void AddFile(FileInfo fileInfo)
        {
            AddFiles(new List<FileInfo> { fileInfo ?? throw new BackupsException("Null argument") });
        }

        internal void AddFiles(IReadOnlyList<FileInfo> fileInfos)
        {
            _files.AddRange(fileInfos ?? throw new BackupsException("Null argument"));
        }
    }
}