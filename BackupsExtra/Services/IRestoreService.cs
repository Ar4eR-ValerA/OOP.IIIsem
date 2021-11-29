using Backups.Entities;
using Backups.Interfaces;

namespace BackupsExtra.Services
{
    public interface IRestoreService
    {
        ILogger Logger { get; }
        BackupJob BackupJob { get; }

        void Restore(RestorePoint restorePoint, string targetDirPath);

        void Restore(RestorePoint restorePoint);
    }
}