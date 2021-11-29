using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class RemoteRestore : IRestoreService
    {
        public RemoteRestore(BackupJob backupJob, ILogger logger)
        {
            Logger = logger ?? throw new BackupsExtraException("Logger is null");
            BackupJob = backupJob ?? throw new BackupsExtraException("Backup job is null");
        }

        public ILogger Logger { get; }
        public BackupJob BackupJob { get; }

        public void Restore(RestorePoint restorePoint, string targetDirPath)
        {
            if (restorePoint is null)
            {
                throw new BackupsExtraException("Restore point is null");
            }

            if (targetDirPath is null)
            {
                throw new BackupsExtraException("Target directory is null");
            }

            if (restorePoint.Storage is not FileServerStorage fileServerStorage)
            {
                throw new BackupsExtraException("This not a server storage");
            }

            FileService.TakeFile(fileServerStorage, Path.GetFileName(restorePoint.Storage.Path));
            ZipFile.ExtractToDirectory(Path.GetFileName(restorePoint.Storage.Path), "temp");

            RestoreStorage(new DirectoryStorage("temp"), targetDirPath);
            Directory.Delete("temp", true);

            Logger.Log($"Restore point {restorePoint.Name} was restored");
        }

        public void Restore(RestorePoint restorePoint)
        {
            if (restorePoint is null)
            {
                throw new BackupsExtraException("Restore point is null");
            }

            if (!BackupJob.RestorePoints.Contains(restorePoint))
            {
                throw new BackupsExtraException("There is no such restore point");
            }

            if (restorePoint.Storage is not FileServerStorage fileServerStorage)
            {
                throw new BackupsExtraException("This not a server storage");
            }

            FileService.TakeFile(fileServerStorage, Path.GetFileName(restorePoint.Storage.Path));

            if (Directory.Exists("temp"))
            {
                Directory.Delete("temp");
            }

            Directory.CreateDirectory("temp");
            ZipFile.ExtractToDirectory(Path.GetFileName(restorePoint.Storage.Path), "temp");

            foreach (string filePath in Directory.GetFiles("temp"))
            {
                string targetPath = BackupJob.OriginalPaths[Path.GetFileNameWithoutExtension(filePath)];
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }

                File.Move(filePath, targetPath);
            }

            Directory.Delete("temp", true);

            Logger.Log($"Restore point {restorePoint.Name} was restored");
        }

        private void RestoreStorage(IStorage storage, string targetDirPath)
        {
            foreach (string storagePath in storage.FilePaths)
            {
                string targetPath = $@"{targetDirPath}\{Path.GetFileName(storagePath)}";

                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }

                File.Copy(storagePath, targetPath);
            }
        }
    }
}