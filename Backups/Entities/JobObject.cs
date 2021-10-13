using System.Collections.Generic;
using Backups.Models;
using Backups.Tools;

namespace Backups.Entities
{
    public class JobObject
    {
        private readonly List<FileData> _fileDatas;

        public JobObject(List<FileData> files)
        {
            _fileDatas = files ?? throw new BackupsException("Null argument");
        }

        public JobObject(FileData fileData)
        {
            _fileDatas = new List<FileData> { fileData ?? throw new BackupsException("Null argument") };
        }

        public JobObject()
        {
            _fileDatas = new List<FileData>();
        }

        public IReadOnlyList<FileData> FileDatas => _fileDatas;

        public void AddFile(FileData fileData)
        {
            _fileDatas.Add(fileData ?? throw new BackupsException("Null argument"));
        }

        public void RemoveFile(FileData fileData)
        {
            _fileDatas.Remove(fileData ?? throw new BackupsException("Null argument"));
        }
    }
}