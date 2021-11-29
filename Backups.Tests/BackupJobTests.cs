using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Services;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        [Test]
        [TestCase(2, 2)]
        public void CreateSingleStorages_SingleStoragesCreated(int restorePointsAmount, int storagesAmount)
        {
            string fileInfo1 = @"Test1.txt";
            string fileInfo2 = @"Test2.txt";

            IJobObject jobObject = new FilesJobObject(new List<string> { fileInfo1, fileInfo2 });
            var backupJob = new BackupJob(jobObject, new LocalArchiveService(new TestArchiver()), new ConsoleLogger());

            backupJob.CreateRestorePoint("Test1", new FileStorage("Test1.test"));

            backupJob.JobObject.RemoveFile(fileInfo1);
            backupJob.CreateRestorePoint("Test2", new FileStorage("Test2.test"));

            Assert.AreEqual(restorePointsAmount, backupJob.RestorePoints.Count);
            int storagesCount = backupJob.RestorePoints.Count;
            Assert.AreEqual(storagesAmount, storagesCount);
        }
    }
}