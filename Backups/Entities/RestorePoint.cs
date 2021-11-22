using System;
using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private List<IStorage> _storages;

        public RestorePoint(string name, IStorage storage)
        {
            Name = name ?? throw new BackupsException("Name is null");
            _storages = new List<IStorage> { storage ?? throw new BackupsException("Storage is null") };
            RestoreDate = DateTime.Now;
        }

        public RestorePoint(string name, List<IStorage> storages)
        {
            Name = name ?? throw new BackupsException("Name is null");
            _storages = storages ?? throw new BackupsException("Storages are null");
            RestoreDate = DateTime.Now;
        }

        public RestorePoint(string name, IStorage storage, DateTime createDate)
        {
            Name = name ?? throw new BackupsException("Name is null");
            _storages = new List<IStorage> { storage ?? throw new BackupsException("Storage is null") };
            RestoreDate = createDate;
        }

        public RestorePoint(string name, List<IStorage> storages, DateTime createDate)
        {
            Name = name ?? throw new BackupsException("Name is null");
            _storages = storages ?? throw new BackupsException("Storages are null");
            RestoreDate = createDate;
        }

        public string Name { get; }
        public DateTime RestoreDate { get; }
        public IReadOnlyList<IStorage> Storages => _storages;

        public void AddStorage(IStorage storage)
        {
            if (storage is null)
            {
                throw new BackupsException("Storage is null");
            }

            _storages.Add(storage);
        }

        public void EraseStorage(IStorage storage)
        {
            if (storage is null)
            {
                throw new BackupsException("Storage is null");
            }

            _storages.Remove(storage);
        }
    }
}