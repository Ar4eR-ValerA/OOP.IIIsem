using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
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
                RestorePointsControlAlgorithm.EraseIrrelevantRestorePoints(backupJob.RestorePoints);
            backupJob.EraseRestorePoints(backupJob.RestorePoints
                .Where(restorePoint => !relevantRestorePoints.Contains(restorePoint)).ToList());
        }

        public RestorePoint MergeRestorePoints(RestorePoint targetRestorePoint, List<RestorePoint> extraRestorePoints)
        {
            foreach (RestorePoint extraRestorePoint in extraRestorePoints)
            {
                return null;
            }

            return null;
        }
    }
}