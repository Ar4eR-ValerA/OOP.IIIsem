using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Backups.Client.Services;
using Backups.Entities;
using Backups.Entities.JobObjects;
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
            var backupJob = new BackupJob(jobObject);

            RestorePoint restorePoint = backupJob.CreateRestorePoint("Test restore point");

            var archiveService = new ServerArchiveService(
                new ZipArchiver(), 
                new IPAddress(new byte[] { 127, 0, 0, 1 }),
                8888); 
            archiveService.ArchiveSingleMode(restorePoint, @"D:\Test.zip");

            archiveService.ArchiveSplitMode(restorePoint, @"D:\");
            
            fileInfo1.Delete();
            fileInfo2.Delete();
        }
    }
}