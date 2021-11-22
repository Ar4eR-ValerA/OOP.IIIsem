using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            RestorePoint targetRestorePoint = backupJob.RestorePoints.Last();
            List<RestorePoint> relevantRestorePoints =
                RestorePointsControlAlgorithm.GetRelevantRestorePoints(backupJob.RestorePoints);

            if (relevantRestorePoints.Count == 0)
            {
                throw new BackupsExtraException("There is no relevant restore points");
            }

            var extraRestorePoints = backupJob.RestorePoints
                .Where(restorePoint => !relevantRestorePoints.Contains(restorePoint)).ToList();

            foreach (RestorePoint extraRestorePoint in extraRestorePoints)
            {
                if (extraRestorePoint.Storage.GetType() == typeof(FileStorage))
                {
                    File.Delete(extraRestorePoint.Storage.Path);
                    backupJob.EraseRestorePoint(extraRestorePoint);
                    continue;
                }

                foreach (FileInfo fileInfo in extraRestorePoint.Storage.FileInfos)
                {
                    if (targetRestorePoint.ContainsFile(fileInfo.Name))
                    {
                        fileInfo.Delete();
                    }
                    else
                    {
                        fileInfo.MoveTo($"{targetRestorePoint.Storage.Path}/{fileInfo.Name}");
                    }
                }

                if (extraRestorePoint.Storage.FileInfos.Count == 0)
                {
                    backupJob.EraseRestorePoint(extraRestorePoint);
                }
            }
        }
    }
}