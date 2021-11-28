using System.IO;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public static class StorageMerge
    {
        public static void Merge(IStorage storage, RestorePoint targetRestorePoint)
        {
            if (storage is null)
            {
                throw new BackupsExtraException("Storage is null");
            }

            if (targetRestorePoint is null)
            {
                throw new BackupsExtraException("Target restore point is null");
            }

            Directory.CreateDirectory("temp");

            if (storage is FileServerStorage serverStorage)
            {
                FileService.TakeFile(
                    serverStorage,
                    $@"temp\{Path.GetFileName(targetRestorePoint.Storage.Path)}");
            }

            if (storage is FileStorage)
            {
                File.Delete(storage.Path);
            }

            if (storage is DirectoryStorage)
            {
                foreach (string filePath in storage.FilePaths)
                {
                    File.Move(filePath, $@"temp\{Path.GetFileName(filePath)}");
                }
            }

            if (targetRestorePoint.Storage is FileServerStorage targetServerStorage)
            {
                foreach (string fileName in Directory.GetFiles("temp"))
                {
                    FileService.SendFile(new FileInfo(fileName), targetServerStorage);
                }
            }
            else
            {
                foreach (string fileName in Directory.GetFiles("temp"))
                {
                    if (!File.Exists($@"{targetRestorePoint.Storage.Path}\{Path.GetFileName(fileName)}"))
                    {
                        File.Move(fileName, $@"{targetRestorePoint.Storage.Path}\{Path.GetFileName(fileName)}");
                    }
                }
            }

            Directory.Delete("temp", true);
        }
    }
}