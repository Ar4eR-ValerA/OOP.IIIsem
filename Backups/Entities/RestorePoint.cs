using System;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(string name, IStorage storage)
        {
            Name = name ?? throw new BackupsException("Name is null");
            Storage = storage ?? throw new BackupsException("Storage is null");
            RestoreDate = DateTime.Now;
        }

        public RestorePoint(string name, IStorage storage, DateTime createDate)
        {
            Name = name ?? throw new BackupsException("Name is null");
            Storage = storage ?? throw new BackupsException("Storage is null");
            RestoreDate = createDate;
        }

        public string Name { get; }
        public DateTime RestoreDate { get; }
        public IStorage Storage { get; }

        public bool ContainsFile(string fileName)
        {
            if (fileName is null)
            {
                throw new BackupsException("File name is null");
            }

            return Storage.FileInfos.Count(f => f.Name == fileName) == 1;
        }
    }
}