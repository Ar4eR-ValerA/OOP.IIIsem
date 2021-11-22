using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRestorePointsControlAlgorithm
    {
        List<RestorePoint> GetRelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints);
    }
}