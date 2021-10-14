using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly List<FileData> _localFileDatas;

        public RestorePoint(string name)
        {
            Name = name ?? throw new BackupsException("Null argument");
            _localFileDatas = new List<FileData>();
            RemoteStorage = new Storage();
        }

        public string Name { get; }
        public IReadOnlyList<FileData> LocalFileDatas => _localFileDatas;
        public Storage RemoteStorage { get; }

        internal void AddLocalFile(FileData fileData)
        {
            AddLocalFiles(new List<FileData> { fileData ?? throw new BackupsException("Null argument") });
        }

        internal void AddLocalFiles(IReadOnlyList<FileData> fileDatas)
        {
            _localFileDatas.AddRange(fileDatas ?? throw new BackupsException("Null argument"));
        }
    }
}