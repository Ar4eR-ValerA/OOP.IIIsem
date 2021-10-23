using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class LocalArchiveService : IArchiveService
    {
        public LocalArchiveService(IArchiver archiver)
        {
            Archiver = archiver;
        }

        public IArchiver Archiver { get; }

        public void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint)
        {
            if (restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            Archiver.Archive(jobObject.FileInfos, restorePoint.Storage.Path);
        }
    }
}