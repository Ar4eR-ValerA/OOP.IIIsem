using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IArchiveService
    {
        public IArchiver Archiver { get; }

        public void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint);
    }
}