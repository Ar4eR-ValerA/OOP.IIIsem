using System.Text.Json.Serialization;

namespace BackupsExtra.Entities
{
    public class SerializedRestorePoint
    {
        [JsonConstructor]
        public SerializedRestorePoint(string name, string storagePath, string storageType)
        {
            Name = name;
            StoragePath = storagePath;
            StorageType = storageType;
        }

        public string Name { get; }
        public string StoragePath { get; }
        public string StorageType { get; }
    }
}