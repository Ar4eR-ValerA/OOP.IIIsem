using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Interfaces;
using Backups.Tools;

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

        public IArchiveService GetArchiveService()
        {
            string archiveServiceTypePath = JsonSerializer.Deserialize<string>(ArchiveServiceType);
            string archiverTypePath = JsonSerializer.Deserialize<string>(ArchiverType);
            string package = archiveServiceTypePath?.Split(".")[0];
            var archiveServiceType = Type.GetType($"{archiveServiceTypePath}, {package}");
            var archiverType = Type.GetType($"{archiverTypePath}, {package}");

            if (archiveServiceType is null || archiverType is null)
            {
                throw new BackupsException("There is no such type");
            }

            var archiveService = (IArchiveService)Activator.CreateInstance(archiveServiceType);
            var archiver = (IArchiver)Activator.CreateInstance(archiverType);
            archiveService!.Archiver = archiver;

            return archiveService;
        }
    }
}