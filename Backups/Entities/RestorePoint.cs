using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        // TODO: Тут не должно быть локальных файлов. Если ресторе поинт создана, то файлы уже отправлены.
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

        public void AddLocalFile(FileInfo fileInfo)
        {
            AddLocalFiles(new List<FileInfo> { fileInfo ?? throw new BackupsException("Null argument") });
        }

        public void AddLocalFiles(IReadOnlyList<FileInfo> fileInfos)
        {
            _localFileInfos.AddRange(fileInfos ?? throw new BackupsException("Null argument"));
        }

        public void AddStorage(IStorage storage)
        {
            _remoteStorages.Add(storage ?? throw new BackupsException("Null argument"));
        }

        public void RemoveStorage(IStorage storage)
        {
            _remoteStorages.Remove(storage ?? throw new BackupsException("Null argument"));
        }
    }
}