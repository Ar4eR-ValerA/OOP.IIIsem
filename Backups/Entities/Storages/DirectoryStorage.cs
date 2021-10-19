using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class DirectoryStorage : IStorage
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryStorage(DirectoryInfo directory)
        {
            _directoryInfo = directory ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => _directoryInfo.GetFiles();
        public string Path => _directoryInfo.FullName;
    }
}