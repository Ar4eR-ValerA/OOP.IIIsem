using System.IO;
using System.Net;
using Backups.Client.Interfaces;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Client.Services
{
    public class ServerArchiveService : IServerArchiveService
    {
        public ServerArchiveService(IArchiver archiver, IPAddress ipAddress, int port)
        {
            Archiver = archiver ?? throw new BackupsException("Archiver is null");
            IpAddress = ipAddress ?? throw new BackupsException("IpAddress is null");
            Port = port;
        }

        public IArchiver Archiver { get; }

        public IPAddress IpAddress { get; }

        public int Port { get; }

        public void ArchiveRestorePoint(IJobObject jobObject, RestorePoint restorePoint)
        {
            if (restorePoint is null)
            {
                throw new BackupsException("RestorePoint is null");
            }
            
            if (jobObject is null)
            {
                throw new BackupsException("JobObject is null");
            }

            string tempDirPath = "temp";
            Archiver.Archive(jobObject.FileInfos, tempDirPath);
            var tempDir = new DirectoryInfo(tempDirPath);

            foreach (FileInfo localFileInfo in tempDir.GetFiles())
            {
                var serverFileInfo = new FileInfo(@$"{restorePoint.Storage.Path} {localFileInfo.Name}");

                FileSender.SendFile(localFileInfo, new FileServerStorage(serverFileInfo, IpAddress, Port));
            }

            tempDir.Delete(true);
        }
    }
}