using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.JobObjects
{
    public class FilesJobObject : IJobObject
    {
        private readonly List<FileInfo> _fileInfos;

        public FilesJobObject(List<FileInfo> files)
        {
            _fileInfos = files ?? throw new BackupsException("Null argument");
        }

        public FilesJobObject(FileInfo fileInfo)
        {
            _fileInfos = new List<FileInfo> { fileInfo ?? throw new BackupsException("Null argument") };
        }

        public FilesJobObject()
        {
            _fileInfos = new List<FileInfo>();
        }

        public IReadOnlyList<FileInfo> FileInfos => _fileInfos;

        public void AddFile(FileInfo fileInfo)
        {
            _fileInfos.Add(fileInfo ?? throw new BackupsException("Null argument"));
        }

        public void RemoveFile(FileInfo fileInfo)
        {
            _fileInfos.Remove(fileInfo ?? throw new BackupsException("Null argument"));
        }
    }
}