using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class SerializedStorage
    {
        [JsonConstructor]
        public SerializedStorage(string storageFile, string storageType)
        {
            StorageFile = storageFile;
            StorageType = storageType;
        }

        public string StorageFile { get; }
        public string StorageType { get; }

        public IStorage GetStorage()
        {
            string storagePath = JsonSerializer.Deserialize<string>(StorageFile);
            string storageTypePath = StorageType;
            string storageTypePackage = storageTypePath?.Split(".")[0];

            var storageType = Type.GetType($"{storageTypePath}, {storageTypePackage}");

            if (storageType is null)
            {
                throw new BackupsExtraException("There is no such type");
            }

            var serverStorage = Activator.CreateInstance(storageType) as IStorage;

            if (serverStorage is null)
            {
                throw new BackupsExtraException("Json error, there is no storage");
            }

            serverStorage.Path = storagePath;
            return serverStorage;
        }
    }
}