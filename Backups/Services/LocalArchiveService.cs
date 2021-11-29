using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Services
{
    public class LocalArchiveService : IArchiveService
    {
        [JsonConstructor]
        public LocalArchiveService()
        {
        }

        public LocalArchiveService(IArchiver archiver)
        {
            Archiver = archiver ?? throw new BackupsException("Archiver is null");
        }

        [JsonProperty]
        public IArchiver Archiver { get; set; }

        public void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint)
        {
            if (restorePoint is null)
            {
                throw new BackupsException("RestorePoint is null");
            }

            if (restorePoint is null)
            {
                throw new BackupsException("JobObject is null");
            }

            if (Archiver is null)
            {
                throw new BackupsException("There is no archiver");
            }

            Archiver.Archive(jobObject.FilePaths, restorePoint.Storage.Path);
        }
    }
}