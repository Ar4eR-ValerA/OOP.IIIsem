using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using Backups.Client.Interfaces;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Client.Services
{
    public class ServerArchiveService : IServerArchiveService
    {
        public ServerArchiveService(IArchiver archiver, string ipAddress, int port)
        {
            Archiver = archiver ?? throw new BackupsException("Archiver is null");
            IpAddress = ipAddress ?? throw new BackupsException("IpAddress is null");
            Port = port;
        }
        
        [JsonConstructor]
        public ServerArchiveService()
        {
        }


        public IArchiver Archiver { get; set; }

        public string IpAddress { get; private set; }

        public int Port { get; private set; }

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
            Archiver.Archive(jobObject.FilePaths, tempDirPath);
            var tempDir = new DirectoryInfo(tempDirPath);

            foreach (FileInfo localFileInfo in tempDir.GetFiles())
            {
                string serverFileInfo = @$"{restorePoint.Storage.Path} {localFileInfo.Name}";
                
                if (restorePoint.Storage.Path.EndsWith(@"\"))
                {
                    serverFileInfo = @$"{restorePoint.Storage.Path}{localFileInfo.Name}";
                }

                FileService.SendFile(localFileInfo, new FileServerStorage(serverFileInfo, IpAddress, Port));
            }

            tempDir.Delete(true);
        }
    }
}