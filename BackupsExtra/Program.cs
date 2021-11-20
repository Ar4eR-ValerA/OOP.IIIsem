using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Services;
using Backups.Tools;
using BackupsExtra.Services;

namespace BackupsExtra
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

            var backupJobSerialize = new SerializeBackupJob();

            backupJobSerialize.Serialize(backupJob1, "cfg");
            BackupJob backupJob2 = backupJobSerialize.Deserialize("cfg");
        }
    }
}