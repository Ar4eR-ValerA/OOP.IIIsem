using System;
using System.IO;
using System.Linq;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

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

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public DateTime RestoreDate { get; private set; }

        [JsonProperty]
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