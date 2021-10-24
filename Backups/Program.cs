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
            var archiveService1 = new LocalArchiveService(new SingleZipArchiver());
            var backupJob1 = new BackupJob(jobObject, archiveService1);

            RestorePoint restorePoint1 = backupJob1.CreateRestorePoint(
                "Test 1",
                new FileStorage(
                    new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test.zip")));

            var archiveService2 = new LocalArchiveService(new SplitZipArchiver());
            var backupJob2 = new BackupJob(jobObject, archiveService2);
            backupJob2.CreateRestorePoint(
                "Test 2",
                new DirectoryStorage(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))));

            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}