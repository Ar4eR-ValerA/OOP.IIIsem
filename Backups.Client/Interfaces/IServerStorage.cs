using System.Net;
using Backups.Interfaces;

namespace Backups.Client.Interfaces
{
    public interface IServerStorage : IStorage
    {
        IPAddress IpAddress { get; }
        int Port { get; }
    }
}