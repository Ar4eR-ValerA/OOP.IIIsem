using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class LocalArchiveService : IArchiveService
    {
        public LocalArchiveService()
        {
            Archiver = null;
        }

        public LocalArchiveService(IArchiver archiver)
        {
            Archiver = archiver ?? throw new BackupsException("Archiver is null");
        }

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

            Archiver.Archive(jobObject.FileInfos, restorePoint.Storage.Path);
        }
    }
}