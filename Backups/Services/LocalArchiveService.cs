using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class LocalArchiveService : IArchiveService
    {
        private IArchiver _archiver;

        public LocalArchiveService(IArchiver archiver)
        {
            Archiver = archiver;
        }

        public IArchiver Archiver
        {
            get => _archiver;
            set => _archiver = value ?? throw new BackupsException("Null argument");
        }

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