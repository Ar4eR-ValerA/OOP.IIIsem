using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Backups.Entities;
using Backups.Interfaces;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class SerializeBackupJob
    {
        public SerializeBackupJob()
        {
            SerializedRestorePoints = new List<SerializedRestorePoint>();
        }

        private List<SerializedRestorePoint> SerializedRestorePoints { get; set; }
        private SerializedJobObject SerializedJobObject { get; set; }
        private SerializedArchiveService SerializedArchiveService { get; set; }

        public void Serialize(BackupJob backupJob, string path)
        {
            SerializeRestorePoints(backupJob.RestorePoints);
            SerializeJobObject(backupJob.JobObject);
            SerializeArchiveService(backupJob.ArchiveService);

            string jsonRestorePoints = JsonSerializer.Serialize(SerializedRestorePoints);
            string jsonJobObject = JsonSerializer.Serialize(SerializedJobObject);
            string jsonArchiveService = JsonSerializer.Serialize(SerializedArchiveService);

            string jsonBackupJob = JsonSerializer.Serialize(new SerializedBackupJob(
                jsonRestorePoints,
                jsonJobObject,
                jsonArchiveService));
            File.WriteAllText(path, jsonBackupJob);
        }

        public BackupJob Deserialize(string path)
        {
            string jsonString = File.ReadAllText(path);
            SerializedBackupJob serializedBackupJob = JsonSerializer.Deserialize<SerializedBackupJob>(jsonString);

            if (serializedBackupJob is null)
            {
                throw new BackupsExtraException($"Where is no such file: {path}");
            }

            SerializedRestorePoints =
                JsonSerializer.Deserialize<List<SerializedRestorePoint>>(serializedBackupJob.SerializedRestorePoints);
            SerializedJobObject =
                JsonSerializer.Deserialize<SerializedJobObject>(serializedBackupJob.SerializedJobObject);
            SerializedArchiveService =
                JsonSerializer.Deserialize<SerializedArchiveService>(serializedBackupJob.SerializedArchiveService);

            if (SerializedJobObject is null || SerializedArchiveService is null || SerializedRestorePoints is null)
            {
                throw new BackupsExtraException("Json error, there is no serialized objects");
            }

            IJobObject jobObject = SerializedJobObject.GetJobObject();
            IArchiveService archiveService = SerializedArchiveService.GetArchiveService();
            var restorePoints = SerializedRestorePoints
                .Select(serializedRestorePoint => serializedRestorePoint.GetRestorePoint()).ToList();

            var backupJop = new BackupJob(jobObject, archiveService);
            backupJop.AddRestorePoints(restorePoints);

            return backupJop;
        }

        private void SerializeRestorePoints(IReadOnlyList<RestorePoint> restorePoints)
        {
            SerializedRestorePoints = new List<SerializedRestorePoint>();

            foreach (RestorePoint restorePoint in restorePoints)
            {
                SerializedRestorePoints.Add(new SerializedRestorePoint(
                    restorePoint.Name,
                    JsonSerializer.Serialize(
                        restorePoint.Storages.Select(s => new SerializedStorage(
                            JsonSerializer.Serialize(s.Path),
                            s.GetType().ToString())).ToList()),
                    restorePoint.RestoreDate));
            }
        }

        private void SerializeJobObject(IJobObject jobObject)
        {
            SerializedJobObject = new SerializedJobObject(
                JsonSerializer.Serialize(jobObject.FileInfos.Select(file => file.FullName).ToList()),
                JsonSerializer.Serialize(jobObject.GetType().ToString()));
        }

        private void SerializeArchiveService(IArchiveService archiveService)
        {
            SerializedArchiveService = new SerializedArchiveService(
                JsonSerializer.Serialize(archiveService.GetType().ToString()),
                JsonSerializer.Serialize(archiveService.Archiver.GetType().ToString()));
        }
    }
}