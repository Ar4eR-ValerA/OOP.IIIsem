using System.IO;
using System.Net;
using Backups.Client.Interfaces;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Client.Services
{
    public class ServerArchiveServiceSingleMode : IServerArchiveService
    {
        public ServerArchiveServiceSingleMode(IArchiver archiver, IPAddress ipAddress, int port)
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
        public void ArchiveRestorePoint(RestorePoint restorePoint, string path)
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
    }
}