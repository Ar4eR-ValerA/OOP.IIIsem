using System.Collections.Generic;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints;
        private IJobObject _jobObject;
        private IArchiveService _archiveService;

        public BackupJob(IJobObject jobObject, IArchiveService archiveService)
        {
            _restorePoints = new List<RestorePoint>();
            JobObject = jobObject;
            ArchiveService = archiveService;
        }

        public IJobObject JobObject
        {
            get => _jobObject;
            set => _jobObject = value ?? throw new BackupsException("Null argument");
        }

        public IArchiveService ArchiveService
        {
            get => _archiveService;
            set => _archiveService = value ?? throw new BackupsException("Null argument");
        }

        public IArchiver Archiver
        {
            get => ArchiveService.Archiver;
            set => ArchiveService.Archiver = value ?? throw new BackupsException("Null argument");
        }

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

        public void ArchiveRestorePoint(RestorePoint restorePoint)
        {
            if (restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            ArchiveService.ArchiveRestorePoint(JobObject, restorePoint);
            _restorePoints.Add(restorePoint);
        }
    }
}