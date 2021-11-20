using System.Text.Json.Serialization;

namespace BackupsExtra.Entities
{
    public class SerializedBackupJob
    {
        [JsonConstructor]
        public SerializedBackupJob(
            string serializedRestorePoints,
            string serializedJobObject,
            string serializedArchiveService)
        {
            SerializedRestorePoints = serializedRestorePoints;
            SerializedJobObject = serializedJobObject;
            SerializedArchiveService = serializedArchiveService;
        }

        public string SerializedRestorePoints { get; }
        public string SerializedJobObject { get; }
        public string SerializedArchiveService { get; }
    }
}