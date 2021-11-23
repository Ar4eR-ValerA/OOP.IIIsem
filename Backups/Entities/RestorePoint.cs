using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        [JsonConstructor]
        public RestorePoint()
        {
        }

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

        public string Name { get; private set; }
        public DateTime RestoreDate { get; private set; }
        public IStorage Storage { get; private set; }

        public bool ContainsFile(string fileName)
        {
            if (fileName is null)
            {
                throw new BackupsException("File name is null");
            }

            return Storage.FilePaths.Count(f => Path.GetFileName(f) == fileName) == 1;
        }
    }
}