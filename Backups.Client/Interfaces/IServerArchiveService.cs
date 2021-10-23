using System.Net;
using Backups.Interfaces;

namespace Backups.Client.Interfaces
{
    public interface IServerArchiveService : IArchiveService
    {
        public IPAddress IpAddress { get; }
        public int Port { get; }
    }
}