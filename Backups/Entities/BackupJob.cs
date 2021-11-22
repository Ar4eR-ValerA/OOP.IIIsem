using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(IJobObject jobObject, IArchiveService archiveService)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject ?? throw new BackupsException("JobObject is null");
            ArchiveService = archiveService ?? throw new BackupsException("ArchiveService is null");
        }

        public IJobObject JobObject { get; }

        public IArchiveService ArchiveService { get; }

        public IArchiver Archiver => ArchiveService.Archiver;

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name, IStorage storage)
        {
            var restorePoint = new RestorePoint(
                name ?? throw new BackupsException("Name is null"),
                storage ?? throw new BackupsException("Storage is null"));

            ArchiveService.ArchiveRestorePoint(JobObject, restorePoint);
            _restorePoints.Add(restorePoint);

            return restorePoint;
        }

        public void AddRestorePoints(List<RestorePoint> restorePoints)
        {
            _restorePoints.AddRange(restorePoints ?? throw new BackupsException("Restore points are null"));
        }

        public void EraseRestorePoint(RestorePoint restorePoint)
        {
            _restorePoints.Remove(restorePoint ?? throw new BackupsException("Restore point is null"));
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
            }
        }
    }
}