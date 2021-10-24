using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IArchiveService
    {
        IArchiver Archiver { get; }

        void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint);
    }
}