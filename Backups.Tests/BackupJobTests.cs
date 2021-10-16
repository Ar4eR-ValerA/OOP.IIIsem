using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Interfaces;
using Backups.Services;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        [Test]
        public void CreateSplitStorages_SplitStoragesCreated()
        {
            var fileInfo1 = new FileInfo(@"Test1.txt");
            var fileInfo2 = new FileInfo(@"Test2.txt");

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint1 = backupJob.CreateRestorePoint("Test restore point 1");

            var archiveService = new ArchiveService(new TestArchiver());
            archiveService.ArchiveSplitMode(
                restorePoint1,
                "Test");

            backupJob.JobObject.RemoveFile(fileInfo1);
            RestorePoint restorePoint2 = backupJob.CreateRestorePoint("Test restore point 2");

            archiveService.ArchiveSplitMode(
                restorePoint2,
                "Test");

            Assert.AreEqual(2, backupJob.RestorePoints.Count);
            
            int storagesCount = backupJob.RestorePoints.Sum(point => point.RemoteStorages.Count);
            Assert.AreEqual(2, storagesCount);
        }

        [Test]
        public void CreateSingleStorages_SingleStoragesCreated()
        {
            var fileInfo1 = new FileInfo(@"Test1.txt");
            var fileInfo2 = new FileInfo(@"Test2.txt");

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint1 = backupJob.CreateRestorePoint("Test restore point 1");

            var archiveService = new ArchiveService(new TestArchiver());
            archiveService.ArchiveSingleMode(
                restorePoint1,
                @"Test.test");

            backupJob.JobObject.RemoveFile(fileInfo1);
            RestorePoint restorePoint2 = backupJob.CreateRestorePoint("Test restore point 2");

            archiveService.ArchiveSingleMode(restorePoint2,
                @"Test.test");

            Assert.AreEqual(2, backupJob.RestorePoints.Count);
            int storagesCount = backupJob.RestorePoints.Sum(point => point.RemoteStorages.Count);
            Assert.AreEqual(2, storagesCount);
        }
    }
}