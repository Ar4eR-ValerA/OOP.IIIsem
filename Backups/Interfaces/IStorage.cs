using System.Collections.Generic;

namespace Backups.Interfaces
{
    public interface IStorage
    {
        IReadOnlyList<string> FilePaths { get; }
        string Path { get; }
    }
}