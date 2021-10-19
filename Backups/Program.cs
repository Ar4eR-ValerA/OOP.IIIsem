using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Services;
using Backups.Tools;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var fileInfo1 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test1.txt");
            var fileInfo2 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test2.txt");

            fileInfo1.Create().Close();
            fileInfo2.Create().Close();
            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var archiveService = new LocalArchiveService(new SingleZipArchiver());
            var backupJob = new BackupJob(jobObject, archiveService);

            RestorePoint restorePoint = backupJob.CreateRestorePoint("Test restore point");
            backupJob.Archive(
                restorePoint,
                new FileStorage(new FileInfo(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test.zip")));

            backupJob.Archiver = new SplitZipArchiver();
            backupJob.Archive(
                restorePoint,
                new DirectoryStorage(new DirectoryInfo(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop))));

            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}