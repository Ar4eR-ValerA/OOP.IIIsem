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
        [TestCase(2, 2)]
        public void CreateSplitStorages_SplitStoragesCreated(int restorePointsAmount, int storagesAmount)
        {
            var fileInfo1 = new FileInfo(@"Test1.txt");
            var fileInfo2 = new FileInfo(@"Test2.txt");

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint1 = backupJob.CreateRestorePoint("Test restore point 1");

            var archiveService = new LocalArchiveServiceSplitMode(new TestArchiver());
            archiveService.ArchiveRestorePoint(
                restorePoint1,
                "Test");

            backupJob.JobObject.RemoveFile(fileInfo1);
            RestorePoint restorePoint2 = backupJob.CreateRestorePoint("Test restore point 2");

            archiveService.ArchiveRestorePoint(
                restorePoint2,
                "Test");

            Assert.AreEqual(restorePointsAmount, backupJob.RestorePoints.Count);
            
            int storagesCount = backupJob.RestorePoints.Sum(point => point.RemoteStorages.Count);
            Assert.AreEqual(storagesAmount, storagesCount);
        }

        [Test]
        [TestCase(2, 2)]
        public void CreateSingleStorages_SingleStoragesCreated(int restorePointsAmount, int storagesAmount)
        {
            var fileInfo1 = new FileInfo(@"Test1.txt");
            var fileInfo2 = new FileInfo(@"Test2.txt");

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint1 = backupJob.CreateRestorePoint("Test restore point 1");

            var archiveService = new LocalArchiveServiceSingleMode(new TestArchiver());
            archiveService.ArchiveRestorePoint(
                restorePoint1,
                @"Test.test");

            backupJob.JobObject.RemoveFile(fileInfo1);
            RestorePoint restorePoint2 = backupJob.CreateRestorePoint("Test restore point 2");

            archiveService.ArchiveRestorePoint(restorePoint2,
                @"Test.test");

            Assert.AreEqual(restorePointsAmount, backupJob.RestorePoints.Count);
            int storagesCount = backupJob.RestorePoints.Sum(point => point.RemoteStorages.Count);
            Assert.AreEqual(storagesAmount, storagesCount);
        }
    }
}