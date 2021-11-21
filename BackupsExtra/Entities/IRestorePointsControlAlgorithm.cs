using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IRestorePointsControlAlgorithm
    {
        List<RestorePoint> EraseIrrelevantRestorePoints(IReadOnlyList<RestorePoint> restorePoints);
    }
}