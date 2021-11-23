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
            string filePath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test1.txt";
            string filePath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Test2.txt";

            File.Create(filePath1).Close();
            File.Create(filePath2).Close();

            IJobObject jobObject = new FilesJobObject(new List<string> { filePath1, filePath2 });
            var backupJob1 = new BackupJob(
                jobObject,
                new ServerArchiveService(new SingleZipArchiver(), "127.0.0.1", 8888),
                new ConsoleLogger());
            backupJob1.CreateRestorePoint("Test 1", new FileStorage(@"E:\Test.zip"));

            var backupJob2 = new BackupJob(
                jobObject,
                new ServerArchiveService(new SplitZipArchiver(), "127.0.0.1", 8888),
                new ConsoleLogger());
            backupJob2.CreateRestorePoint("Test 2", new DirectoryStorage(@"E:\"));

            File.Delete(filePath1);
            File.Delete(filePath2);
        }
    }
}