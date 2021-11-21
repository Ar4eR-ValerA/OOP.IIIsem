using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Tools;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePointsControlOutDated : IRestorePointsControlAlgorithm
    {
        public RestorePointsControlOutDated(DateTime deadLine)
        {
            DeadLine = deadLine;
        }

        public DateTime DeadLine { get; }

        public List<RestorePoint> EraseIrrelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints)
        {
            if (restorePoints is null)
            {
                throw new BackupsExtraException("Restore points are null");
            }

            var relevantRestorePoints =
                restorePoints.Where(restorePoint => restorePoint.RestoreDate > DeadLine).ToList();
            if (relevantRestorePoints.Count == 0)
            {
                throw new BackupsException("There are no such restore points");
            }

            return relevantRestorePoints;
        }
    }
}