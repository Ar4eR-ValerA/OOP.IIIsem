using System.Collections.Generic;
using Backups.Enums;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(JobObject jobObject, StorageMode storageMode)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject;
            StorageMode = storageMode;
        }

        public StorageMode StorageMode { get; set; }
        public JobObject JobObject { get; }
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name)
        {
            var restorePoint = new RestorePoint(name ?? throw new BackupsException("Null argument"));
            restorePoint.AddLocalFiles(JobObject.FileDatas);

            _restorePoints.Add(restorePoint);

            return restorePoint;
        }
    }
}