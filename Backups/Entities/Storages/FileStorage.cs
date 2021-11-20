using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class FileStorage : IStorage
    {
        private FileInfo _fileInfo;

        public FileStorage()
        {
            _fileInfo = null;
        }

        public FileStorage(FileInfo fileInfo)
        {
            _fileInfo = fileInfo ?? throw new BackupsException("FileInfo is null");
        }

        public IReadOnlyList<FileInfo> FileInfos => new List<FileInfo> { _fileInfo };

        public string Path
        {
            get
            {
                if (_fileInfo is null)
                {
                    throw new BackupsException("There is no file info");
                }

                return _fileInfo.FullName;
            }
            set => _fileInfo = new FileInfo(value ?? throw new BackupsException("value is null"));
        }
    }
}