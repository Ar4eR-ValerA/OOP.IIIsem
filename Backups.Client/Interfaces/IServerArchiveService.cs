using System.Net;
using Backups.Interfaces;

namespace Backups.Client.Interfaces
{
    public interface IServerArchiveService : IArchiveService
    {
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
    }
}