using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.JobObjects
{
    public class FileJobObject : IJobObject
    {
        private readonly FileInfo _fileInfos;

        public FileJobObject(FileInfo fileInfo)
        {
            _fileInfos = fileInfo ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { _fileInfos };
    }
}