using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class SerializedRestorePoint
    {
        [JsonConstructor]
        public SerializedRestorePoint(string name, string serializedStorages, DateTime createDate)
        {
            Name = name ?? throw new BackupsExtraException("Name is null");
            SerializedStorages = serializedStorages ?? throw new BackupsExtraException("Serialized storages are null");
            CreateDate = createDate;
        }

        public string Name { get; }
        public DateTime CreateDate { get; }
        public string SerializedStorages { get; }

        public RestorePoint GetRestorePoint()
        {
            string name = Name;
            List<SerializedStorage> serializedStorages =
                JsonSerializer.Deserialize<List<SerializedStorage>>(SerializedStorages);

            if (serializedStorages is null)
            {
                throw new BackupsExtraException("Json error, there are no serialized storages");
            }

            var storages = serializedStorages.Select(s => s.GetStorage()).ToList();
            return new RestorePoint(name, storages, CreateDate);
        }
    }
}