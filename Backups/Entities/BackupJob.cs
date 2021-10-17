using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(IJobObject jobObject)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject ?? throw new BackupsException("Null argument");
        }

        public IJobObject JobObject { get; }
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name)
        {
            var restorePoint = new RestorePoint(name ?? throw new BackupsException("Null argument"));
            restorePoint.AddLocalFiles(JobObject.FileInfos);

            _restorePoints.Add(restorePoint);

            return restorePoint;
        }
    }
}