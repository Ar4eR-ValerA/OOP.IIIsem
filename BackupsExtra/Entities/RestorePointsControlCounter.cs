using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePointsControlCounter : IRestorePointsControlAlgorithm
    {
        public RestorePointsControlCounter(int amount)
        {
            if (amount < 0)
            {
                throw new BackupsExtraException("Negative amount");
            }

            Amount = amount;
        }

        public int Amount { get; }

        public List<RestorePoint> GetRelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints)
        {
            if (restorePoints is null)
            {
                throw new BackupsExtraException("Restore points are null");
            }

            var relevantRestorePoints = new List<RestorePoint>(restorePoints);
            relevantRestorePoints.RemoveRange(0, restorePoints.Count - Amount);
            return relevantRestorePoints;
        }
    }
}