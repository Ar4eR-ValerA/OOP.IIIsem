using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class DirectoryStorage : IStorage
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryStorage(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo ?? throw new BackupsException("DirectoryInfo is null");
        }

        public IReadOnlyList<FileInfo> FileInfos => _directoryInfo.GetFiles();
        public string Path => _directoryInfo.FullName;
    }
}