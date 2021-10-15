using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class DirectoryStorages : IStorage
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryStorages(DirectoryInfo directory)
        {
            _directoryInfo = directory ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<FileInfo> FileInfos => _directoryInfo.GetFiles();
    }
}