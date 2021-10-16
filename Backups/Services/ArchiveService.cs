using System.IO;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class ArchiveService
    {
        public ArchiveService(IArchiver archiver)
        {
            Archiver = archiver;
        }

        public IArchiver Archiver { get; set; }

        /// <summary>
        /// Archiving files from restore point to single file that indicated in path.
        /// </summary>
        /// <param name="restorePoint"> Restore point which archiving. </param>
        /// /// <param name="path"> Path must points to archive file, which will be created by method. </param>
        public void ArchiveSingleMode(RestorePoint restorePoint, string path)
        {
            if (path is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            Archiver.ArchiveSingleMode(restorePoint.LocalFileInfos, path);

            restorePoint.AddStorage(new FileStorage(new FileInfo(path)));
        }

        public void ArchiveSplitMode(RestorePoint restorePoint, string path)
        {
            // TODO:учесть, что нужно сделать кучу стораджей
        }
    }
}