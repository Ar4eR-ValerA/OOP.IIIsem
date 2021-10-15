using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Interfaces;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        [Test]
        public void Setup()
        {
            var fileInfo1 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test1.txt");
            var fileInfo2 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test2.txt");

            fileInfo1.Create().Close();
            fileInfo2.Create().Close();
            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint = backupJob.CreateRestorePoint("Test restore point");
            ZipArchiver.ArchiveSingleMode(
                restorePoint,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test.zip");

            ZipArchiver.ArchiveSplitMode(restorePoint, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}