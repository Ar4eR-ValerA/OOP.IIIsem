using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IArchiver
    {
        /// <summary>
        /// Archiving files to single file that indicated in path.
        /// </summary>
        /// <param name="fileInfos"> Files which archiving. </param>
        /// <param name="path"> Path must points to archive file, which will be created by method. </param>
        public void ArchiveSingleMode(IReadOnlyList<FileInfo> fileInfos, string path);

        /// <summary>
        /// Archiving each file to his own zip file in directory that indicated in path.
        /// </summary>
        /// <param name="fileInfos"> Files which archiving. </param>
        /// <param name="path">
        /// Path must points to directory where archived files will be located. Method will create directory.
        /// </param>
        public void ArchiveSplitMode(IReadOnlyList<FileInfo> fileInfos, string path);
    }
}