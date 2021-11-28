using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Entities
{
    public class BackupJob
    {
        [JsonProperty("restorePoints")]
        private readonly List<RestorePoint> _restorePoints;

        [JsonProperty("originalPaths")]
        private readonly Dictionary<string, string> _originalPaths;

        public BackupJob(IJobObject jobObject, IArchiveService archiveService, ILogger logger)
        {
            _restorePoints = new List<RestorePoint>();
            _originalPaths = new Dictionary<string, string>();
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
            List<RestorePoint> restorePoints,
            Dictionary<string, string> originalPaths)
        {
            _restorePoints = restorePoints;
            _originalPaths = originalPaths;
            JobObject = jobObject ?? throw new BackupsException("JobObject is null");
            ArchiveService = archiveService ?? throw new BackupsException("ArchiveService is null");
            Logger = logger ?? throw new BackupsException("Logger is null");
            Logger.Log($"Backup job was created");
        }

        [JsonProperty]
        public IJobObject JobObject { get; private set; }

        [JsonProperty]
        public IArchiveService ArchiveService { get; set; }

        [JsonProperty]
        public ILogger Logger { get; private set; }

        [JsonProperty]
        public IReadOnlyDictionary<string, string> OriginalPaths => _originalPaths;

        [JsonIgnore]
        public IArchiver Archiver => ArchiveService.Archiver;

        [JsonIgnore]
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name, IStorage storage)
        {
            var restorePoint = new RestorePoint(
                name ?? throw new BackupsException("Name is null"),
                storage ?? throw new BackupsException("Storage is null"));

            foreach (string path in JobObject.FilePaths)
            {
                _originalPaths[Path.GetFileName(path)] = path;
            }

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