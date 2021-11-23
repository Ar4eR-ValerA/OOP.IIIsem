using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IStorage
    {
        IReadOnlyList<string> FilePaths { get; }
        string Path { get; }
    }
}