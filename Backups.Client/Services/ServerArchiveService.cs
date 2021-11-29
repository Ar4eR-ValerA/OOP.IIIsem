using System.IO;
using System.IO.Compression;
using Backups.Client.Interfaces;
using Backups.Client.ServerStorages;
using Backups.Client.Tools;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

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


        [JsonProperty] public IArchiver Archiver { get; set; }

        [JsonProperty] public string IpAddress { get; private set; }

        [JsonProperty] public int Port { get; private set; }

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

            if (File.Exists("temp.zip"))
            {
                File.Delete("temp.zip");
            }

            ZipFile.CreateFromDirectory(tempDirPath, "temp.zip");

            FileService.SendFile(new FileInfo("temp.zip"), new FileServerStorage(
                restorePoint.Storage.Path, IpAddress, Port));

            tempDir.Delete(true);
            File.Delete("temp.zip");
        }
    }
}