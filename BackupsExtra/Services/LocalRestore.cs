﻿using System.IO;
using Backups.Entities;
using Backups.Interfaces;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class LocalRestore
    {
        public LocalRestore(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

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

            RestoreStorage(restorePoint.Storage, targetDirPath);
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