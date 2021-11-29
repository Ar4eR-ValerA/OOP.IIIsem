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

        public List<RestorePoint> GetRelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints)
        {
            if (restorePoints is null)
            {
                throw new BackupsExtraException("Restore points are null");
            }

            List<RestorePoint> relevantRestorePoints = new RestorePointsControlOutDated(DeadLine)
                .GetRelevantRestorePoints(restorePoints);

            relevantRestorePoints = new RestorePointsControlCounter(Amount)
                .GetRelevantRestorePoints(relevantRestorePoints);

            return relevantRestorePoints;
        }
    }
}