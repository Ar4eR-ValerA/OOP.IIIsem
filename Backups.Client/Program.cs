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
            var backupJob = new BackupJob(
                jobObject, 
                new ServerArchiveService(new SingleZipArchiver(), IPAddress.Parse("127.0.0.1"), 8888));

            RestorePoint restorePoint = backupJob.CreateRestorePoint("Test restore point");
            
            backupJob.Archive(restorePoint, new FileStorage(new FileInfo(@"D:\Test.zip")));

            backupJob.Archiver = new SplitZipArchiver();
            backupJob.Archive(restorePoint, new DirectoryStorage(new DirectoryInfo(@"D:\")));
            
            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}