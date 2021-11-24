using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities
{
    public class BackupJob
    {
        [JsonProperty("restorePoints")]
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(IJobObject jobObject, IArchiveService archiveService, ILogger logger)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject ?? throw new BackupsException("JobObject is null");
            ArchiveService = archiveService ?? throw new BackupsException("ArchiveService is null");
            Logger = logger ?? throw new BackupsException("Logger is null");
            Logger.Log($"Backup job was created");
        }

        [JsonConstructor]
        private BackupJob(
            IJobObject jobObject,
            IArchiveService archiveService,
            ILogger logger,
            List<RestorePoint> restorePoints)
        {
            _restorePoints = restorePoints;
            JobObject = jobObject ?? throw new BackupsException("JobObject is null");
            ArchiveService = archiveService ?? throw new BackupsException("ArchiveService is null");
            Logger = logger ?? throw new BackupsException("Logger is null");
            Logger.Log($"Backup job was created");
        }

        [JsonProperty]
        public IJobObject JobObject { get; private set; }

        [JsonProperty]
        public IArchiveService ArchiveService { get; private set; }

        [JsonProperty]
        public ILogger Logger { get; private set; }

        [JsonIgnore]
        public IArchiver Archiver => ArchiveService.Archiver;

        [JsonIgnore]
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name, IStorage storage)
        {
            var restorePoint = new RestorePoint(
                name ?? throw new BackupsException("Name is null"),
                storage ?? throw new BackupsException("Storage is null"));

            ArchiveService.ArchiveRestorePoint(JobObject, restorePoint);
            _restorePoints.Add(restorePoint);
            Logger.Log($"Restore point {name} was created");

            return restorePoint;
        }

        public void EraseRestorePoint(RestorePoint restorePoint)
        {
            EraseRestorePoints(
                new List<RestorePoint>
                {
                    restorePoint ?? throw new BackupsException("Restore point is null"),
                });
        }

        public void EraseRestorePoints(List<RestorePoint> restorePoints)
        {
            if (restorePoints is null)
            {
                throw new BackupsException("Restore points are null");
            }

            foreach (RestorePoint restorePoint in restorePoints)
            {
                _restorePoints.Remove(restorePoint);
                Logger.Log($"Restore point {restorePoint.Name} was erased");
            }
        }
    }
}