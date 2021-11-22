using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Entities;
using Backups.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class SerializedRestorePoint
    {
        [JsonConstructor]
        public SerializedRestorePoint(string name, string storageFile, string storageType)
        {
            Name = name ?? throw new BackupsExtraException("Name is null");
            StorageFile = storageFile;
            StorageType = storageType;
        }

        public string Name { get; }
        public string StorageFile { get; }
        public string StorageType { get; }

        public RestorePoint GetRestorePoint()
        {
            string name = Name;
            string storagePath = JsonSerializer.Deserialize<string>(StorageFile);
            string storageTypePath = StorageType;
            string storageTypePackage = storageTypePath?.Split(".")[0];

            var storageType = Type.GetType($"{storageTypePath}, {storageTypePackage}");

            if (storageType is null)
            {
                throw new BackupsExtraException("There is no such type");
            }

            var storage = Activator.CreateInstance(storageType) as IStorage;

            if (storage is null)
            {
                throw new BackupsExtraException("Json error, there is no storage");
            }

            storage.Path = storagePath;

            return new RestorePoint(name, storage);
        }
    }
}