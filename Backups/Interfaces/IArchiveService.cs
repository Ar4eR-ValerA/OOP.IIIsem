using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IArchiveService
    {
        public IArchiver Archiver { get; set; }

        /// <summary>
        /// Archiving files from restore point to single file that indicated in path.
        /// Method must add new Storage to RestorePoint.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path"> Path must points to archive file, which will be created by method. </param>
        public void ArchiveSingleMode(RestorePoint restorePoint, string path);

        /// <summary>
        /// Archiving files from restore point to several zip files in directory that indicated in path.
        /// Method must add new Storage to RestorePoint.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path">
        /// Path must points to directory where zip files will be located.
        /// </param>
        public void ArchiveSplitMode(RestorePoint restorePoint, string path);
    }
}