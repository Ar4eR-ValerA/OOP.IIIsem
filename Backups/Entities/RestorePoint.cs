using System;
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

        public string Name { get; }
        public DateTime RestoreDate { get; }
        public IStorage Storage { get; }
    }
}