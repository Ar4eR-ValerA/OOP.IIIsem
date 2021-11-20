using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IArchiveService
    {
        IArchiver Archiver { get; set; }

        void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint);
    }
}