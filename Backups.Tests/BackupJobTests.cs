using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Entities.Storages;
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
            var backupJob = new BackupJob(jobObject, new LocalArchiveService(new SplitTestArchiver()));

            backupJob.ArchiveRestorePoint(new RestorePoint("Test1", new DirectoryStorage(new DirectoryInfo("Test1"))));

            backupJob.JobObject.RemoveFile(fileInfo1);
            backupJob.ArchiveRestorePoint(new RestorePoint("Test2", new DirectoryStorage(new DirectoryInfo("Test2"))));

            Assert.AreEqual(restorePointsAmount, backupJob.RestorePoints.Count);

            int storagesCount = backupJob.RestorePoints.Count;
            Assert.AreEqual(storagesAmount, storagesCount);
        }

        [Test]
        [TestCase(2, 2)]
        public void CreateSingleStorages_SingleStoragesCreated(int restorePointsAmount, int storagesAmount)
        {
            var fileInfo1 = new FileInfo(@"Test1.txt");
            var fileInfo2 = new FileInfo(@"Test2.txt");

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject, new LocalArchiveService(new SingleTestArchiver()));

            backupJob.ArchiveRestorePoint(new RestorePoint("Test1", new FileStorage(new FileInfo("Test1.test"))));

            backupJob.JobObject.RemoveFile(fileInfo1);
            backupJob.ArchiveRestorePoint(new RestorePoint("Test2", new FileStorage(new FileInfo("Test2.test"))));

            Assert.AreEqual(restorePointsAmount, backupJob.RestorePoints.Count);
            int storagesCount = backupJob.RestorePoints.Count;
            Assert.AreEqual(storagesAmount, storagesCount);
        }
    }
}