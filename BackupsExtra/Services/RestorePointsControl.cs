using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Client.ServerStorages;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class RestorePointsControl
    {
        public RestorePointsControl(IRestorePointsControlAlgorithm restorePointsControlAlgorithm)
        {
            RestorePointsControlAlgorithm = restorePointsControlAlgorithm;
        }

        public IRestorePointsControlAlgorithm RestorePointsControlAlgorithm { get; }

        public void EraseExtraRestorePoints(BackupJob backupJob)
        {
            if (backupJob is null)
            {
                throw new BackupsExtraException("Backup job is null");
            }

            List<RestorePoint> relevantRestorePoints =
                RestorePointsControlAlgorithm.GetRelevantRestorePoints(backupJob.RestorePoints);
            backupJob.EraseRestorePoints(backupJob.RestorePoints
                .Where(restorePoint => !relevantRestorePoints.Contains(restorePoint)).ToList());
        }

        public void MergeExtraRestorePoints(BackupJob backupJob)
        {
            if (backupJob is null)
            {
                throw new BackupsExtraException("Backup job is null");
            }

            List<RestorePoint> relevantRestorePoints =
                RestorePointsControlAlgorithm.GetRelevantRestorePoints(backupJob.RestorePoints);

            if (relevantRestorePoints.Count == 0)
            {
                throw new BackupsExtraException("There is no relevant restore points");
            }

            var extraRestorePoints = backupJob.RestorePoints
                .Where(restorePoint => !relevantRestorePoints.Contains(restorePoint)).ToList();

            RestorePoint targetRestorePoint = backupJob.RestorePoints.Last();

            foreach (RestorePoint extraRestorePoint in extraRestorePoints)
            {
                StorageMerge.Merge(extraRestorePoint.Storage, targetRestorePoint);
                backupJob.EraseRestorePoint(extraRestorePoint);
            }

            foreach (string filePath in targetRestorePoint.Storage.FilePaths)
            {
                backupJob.JobObject.AddFile(filePath);
            }
        }
    }
}