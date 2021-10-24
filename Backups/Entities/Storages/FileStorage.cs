using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class FileStorage : IStorage
    {
        private readonly FileInfo _fileInfo;

        public FileStorage(FileInfo fileInfo)
        {
            _fileInfo = fileInfo ?? throw new BackupsException("FileInfo is null");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { _fileInfo };
        public string Path => _fileInfo.FullName;
    }
}