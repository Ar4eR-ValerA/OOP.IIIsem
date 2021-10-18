using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IArchiveService
    {
        public IArchiver Archiver { get; set; }

        /// <summary>
        /// Archiving files from restore point to path that indicated in path.
        /// Method must add new Storage to RestorePoint.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path"> Path must points to archive file, which will be created by method. </param>
        public void ArchiveRestorePoint(RestorePoint restorePoint, string path);
    }
}