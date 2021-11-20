using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class BackupJobsService
    {
        private readonly List<BackupJob> _backupJobs;

        public BackupJobsService()
        {
            _backupJobs = new List<BackupJob>();
        }

        [JsonConstructor]
        public BackupJobsService(List<BackupJob> backupJobs) =>
            _backupJobs = backupJobs;

        public IReadOnlyList<BackupJob> BackupJobs => _backupJobs;

        public void Save(string path)
        {
            if (path is null)
            {
                throw new BackupsExtraException("Path is null");
            }

            string jsonString = JsonSerializer.Serialize(_backupJobs);
            File.WriteAllText(path, jsonString);
        }

        public void Load(string path)
        {
            if (path is null)
            {
                throw new BackupsExtraException("Path is null");
            }

            string buffer = File.ReadAllText(path);
            JsonSerializer.Deserialize<BackupJobsService>(buffer);
        }

        public void AddBackupJob(BackupJob backupJob)
        {
            if (backupJob is null)
            {
                throw new BackupsExtraException("Backup job is null");
            }

            _backupJobs.Add(backupJob);
        }
    }
}