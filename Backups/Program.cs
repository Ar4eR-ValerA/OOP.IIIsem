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
            string filePath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test1.txt";
            string filePath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test2.txt";

            File.Create(filePath1).Close();
            File.Create(filePath2).Close();

            IJobObject jobObject = new FilesJobObject(new List<string> { filePath1, filePath2 });
            var archiveService1 = new LocalArchiveService(new SingleZipArchiver());
            var backupJob1 = new BackupJob(jobObject, archiveService1);

            RestorePoint restorePoint1 = backupJob1.CreateRestorePoint(
                "Test 1",
                new FileStorage(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test.zip"));

            var archiveService2 = new LocalArchiveService(new SplitZipArchiver());
            var backupJob2 = new BackupJob(jobObject, archiveService2);
            backupJob2.CreateRestorePoint(
                "Test 2",
                new DirectoryStorage(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));

            File.Delete(filePath1);
            File.Delete(filePath2);
        }
    }
}