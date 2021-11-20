using System.Text.Json.Serialization;

namespace BackupsExtra.Entities
{
    public class SerializedArchiveService
    {
        [JsonConstructor]
        public SerializedArchiveService(string archiveServiceType, string archiverType)
        {
            ArchiveServiceType = archiveServiceType;
            ArchiverType = archiverType;
        }

        public string ArchiveServiceType { get; }
        public string ArchiverType { get; }
    }
}