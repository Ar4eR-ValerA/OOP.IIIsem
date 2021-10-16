using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly List<FileInfo> _localFileInfos;
        private readonly List<IStorage> _remoteStorages;

        public RestorePoint(string name)
        {
            Name = name ?? throw new BackupsException("Null argument");
            _localFileInfos = new List<FileInfo>();
            _remoteStorages = new List<IStorage>();
        }

        public string Name { get; }
        public IReadOnlyList<FileInfo> LocalFileInfos => _localFileInfos;
        public IReadOnlyList<IStorage> RemoteStorages => _remoteStorages;

        internal void AddLocalFile(FileInfo fileInfo)
        {
            AddLocalFiles(new List<FileInfo> { fileInfo ?? throw new BackupsException("Null argument") });
        }

        internal void AddLocalFiles(IReadOnlyList<FileInfo> fileInfos)
        {
            _localFileInfos.AddRange(fileInfos ?? throw new BackupsException("Null argument"));
        }

        internal void AddStorage(IStorage storage)
        {
            _remoteStorages.Add(storage);
        }
    }
}