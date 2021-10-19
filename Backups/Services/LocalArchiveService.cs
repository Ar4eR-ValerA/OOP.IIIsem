using System.IO;
using Backups.Entities;
using Backups.Entities.Storages;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Services
{
    public class LocalArchiveService : IArchiveService
    {
        public LocalArchiveService(IArchiver archiver)
        {
            Archiver = archiver;
        }

        public IArchiver Archiver { get; set; }

        public void ArchiveRestorePoint(RestorePoint restorePoint, IStorage storage)
        {
            if (storage is null || restorePoint is null)
            {
                throw new BackupsException("Null argument");
            }

            Archiver.Archive(restorePoint.LocalFileInfos, storage.Path);

            restorePoint.AddStorage(storage);
        }
    }
}