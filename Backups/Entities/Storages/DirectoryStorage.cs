using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities.Storages
{
    public class DirectoryStorage : IStorage
    {
        private DirectoryInfo _directoryInfo;

        public DirectoryStorage()
        {
        }

        public DirectoryStorage(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo ?? throw new BackupsException("DirectoryInfo is null");
        }

        public IReadOnlyList<FileInfo> FileInfos
        {
            get
            {
                if (_directoryInfo is null)
                {
                    throw new BackupsException("There is no directory info");
                }

                return _directoryInfo.GetFiles();
            }
        }

        public string Path
        {
            get => _directoryInfo.FullName;
            set => _directoryInfo = new DirectoryInfo(value ?? throw new BackupsException("value is null"));
        }
    }
}