using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IJobObject
    {
        IReadOnlyList<string> FilePaths { get; }

        void AddFile(string filePath);
        void RemoveFile(string filePath);
    }
}