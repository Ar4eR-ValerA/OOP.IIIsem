using System.Collections.Generic;
using System.IO;
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
        }

        public string Name { get; }
        public IStorage Storage { get; }
    }
}