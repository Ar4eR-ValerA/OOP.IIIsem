using System.IO;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class LocalArchiveServiceSplitMode : IArchiveService
    {
        public LocalArchiveServiceSplitMode(IArchiver archiver)
        {
            Archiver = archiver;
        }

        public IArchiver Archiver { get; set; }

        /// <summary>
        /// Archiving files from restore point to several zip files in directory that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// <param name="path">
        /// Path must points to directory where zip files will be located. Method will create directory.
        /// </param>
        public void ArchiveRestorePoint(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            Archiver.ArchiveSplitMode(restorePoint.LocalFileInfos, path);

            restorePoint.AddStorage(new DirectoryStorage(new DirectoryInfo(path)));
        }
    }
}