using System;
using System.IO;
using System.Net;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Client.Services
{
    public class ServerArchiveService
    {
        public ServerArchiveService(IArchiver archiver, IPAddress ipAddress, int port)
        {
            Archiver = archiver ?? throw new BackupsException("Null argument");
            IpAddress = ipAddress ?? throw new BackupsException("Null argument");
            Port = port;
        }

        public IArchiver Archiver { get; set; }
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// Archiving files from restore point to single file that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path">
        /// Path must points to archive file, which will be created by method.
        /// </param>
        public void ArchiveSingleMode(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            string tempFilePath = $"temp{new FileInfo(path).Name}";
            Archiver.ArchiveSingleMode(restorePoint.LocalFileInfos, tempFilePath);
            
            FileSender.SendFile(
                new FileInfo(tempFilePath),
                new FileServerStorage(new FileInfo(path), IpAddress, Port));
            File.Delete(tempFilePath);

            restorePoint.AddStorage(new FileServerStorage(new FileInfo(path), IpAddress, Port));
        }

        /// <summary>
        /// Archiving files from restore point to several zip files in directory that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path">
        /// Path must points to directory where zip files will be located. Method will create directory.
        /// </param>
        public void ArchiveSplitMode(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            string tempDirPath = "temp";
            Archiver.ArchiveSplitMode(restorePoint.LocalFileInfos, tempDirPath);
            var tempDir = new DirectoryInfo(tempDirPath);
            foreach (FileInfo localFileInfo in tempDir.GetFiles())
            {
                var serverFileInfo = new FileInfo(@$"{path}{localFileInfo.Name}");
                
                FileSender.SendFile(localFileInfo, new FileServerStorage(serverFileInfo, IpAddress, Port));
            }
            
            tempDir.Delete(true);

            restorePoint.AddStorage(new DirectoryStorage(new DirectoryInfo(path)));
        }
    }
}