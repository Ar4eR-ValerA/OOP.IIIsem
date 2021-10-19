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

        public IArchiver Archiver
        {
            get => ArchiveService.Archiver;
            set => ArchiveService.Archiver = value;
        }

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public RestorePoint CreateRestorePoint(string name)
        {
            var restorePoint = new RestorePoint(name ?? throw new BackupsException("Null argument"));
            restorePoint.AddLocalFiles(JobObject.FileInfos);

            _restorePoints.Add(restorePoint);

            return restorePoint;
        }

        public void Archive(RestorePoint restorePoint, IStorage storage)
        {
            ArchiveService.ArchiveRestorePoint(restorePoint, storage);
        }
    }
}