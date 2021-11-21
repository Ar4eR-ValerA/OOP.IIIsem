using System;
using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePointsControlHybrid : IRestorePointsControlAlgorithm
    {
        public RestorePointsControlHybrid(int amount, DateTime deadLine)
        {
            if (amount <= 0)
            {
                throw new BackupsExtraException("Amount must be positive");
            }

            Amount = amount;
            DeadLine = deadLine;
        }

        public int Amount { get; }
        public DateTime DeadLine { get; }

        public List<RestorePoint> EraseIrrelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints)
        {
            if (restorePoints is null)
            {
                throw new BackupsExtraException("Restore points are null");
            }

            List<RestorePoint> relevantRestorePoints = new RestorePointsControlOutDated(DeadLine)
                .EraseIrrelevantRestorePoints(restorePoints);

            relevantRestorePoints = new RestorePointsControlCounter(Amount)
                .EraseIrrelevantRestorePoints(relevantRestorePoints);

            return relevantRestorePoints;
        }
    }
}