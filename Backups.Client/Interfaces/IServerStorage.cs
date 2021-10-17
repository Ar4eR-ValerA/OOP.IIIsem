using System.Net;
using Backups.Interfaces;

namespace Backups.Client.Interfaces
{
    public interface IServerStorage : IStorage
    {
        public IPAddress IpAddress { get; }
        public int Port { get; }
    }
}