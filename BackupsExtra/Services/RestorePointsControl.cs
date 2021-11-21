using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Entities;

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
            List<RestorePoint> relevantRestorePoints =
                RestorePointsControlAlgorithm.EraseIrrelevantRestorePoints(backupJob.RestorePoints);
            backupJob.EraseRestorePoints(backupJob.RestorePoints
                .Where(restorePoint => !relevantRestorePoints.Contains(restorePoint)).ToList());
        }
    }
}