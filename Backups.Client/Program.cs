using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Backups.Client.Services;
using Backups.Entities;
using Backups.Entities.JobObjects;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fileInfo1 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test1.txt");
            var fileInfo2 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test2.txt");

            fileInfo1.Create().Close();
            fileInfo2.Create().Close();

            IJobObject jobObject = new FilesJobObject(new List<FileInfo> { fileInfo1, fileInfo2 });
            var backupJob1 = new BackupJob(
                jobObject,
                new ServerArchiveService(new SingleZipArchiver(), IPAddress.Parse("127.0.0.1"), 8888));
            backupJob1.CreateRestorePoint("Test 1", new FileStorage(new FileInfo(@"E:\Test.zip")));

            var backupJob2 = new BackupJob(
                jobObject,
                new ServerArchiveService(new SplitZipArchiver(), IPAddress.Parse("127.0.0.1"), 8888));
            backupJob2.CreateRestorePoint("Test 2", new DirectoryStorage(new DirectoryInfo(@"E:\")));

            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}