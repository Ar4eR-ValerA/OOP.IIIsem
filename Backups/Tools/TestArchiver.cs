using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class TestArchiver : IArchiver
    {
        /// <summary>
        /// Doing nothing.
        /// </summary>
        /// <param name="fileInfos"> Some Files. </param>
        /// <param name="path"> Path to nothing. </param>
        public void ArchiveSingleMode(IReadOnlyList<FileInfo> fileInfos, string path)
        {
        }

        /// <summary>
        /// Doing nothing.
        /// </summary>
        /// <param name="fileInfos"> Some Files. </param>
        /// <param name="path"> Path to nothing. </param>
        public void ArchiveSplitMode(IReadOnlyList<FileInfo> fileInfos, string path)
        {
        }
    }
}