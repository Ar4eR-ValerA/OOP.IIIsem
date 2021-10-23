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
            JobObject = jobObject ?? throw new BackupsException("Null argument");
            ArchiveService = archiveService ?? throw new BackupsException("Null argument");
        }

        public IJobObject JobObject { get; }

        public IArchiveService ArchiveService { get; }

        public IArchiver Archiver => ArchiveService.Archiver;

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name, IStorage storage)
        {
            if (name is null || storage is null)
            {
                throw new BackupsException("Null argument");
            }

            var restorePoint = new RestorePoint(name, storage);
            ArchiveService.ArchiveRestorePoint(JobObject, restorePoint);
            _restorePoints.Add(restorePoint);

            return restorePoint;
        }
    }
}