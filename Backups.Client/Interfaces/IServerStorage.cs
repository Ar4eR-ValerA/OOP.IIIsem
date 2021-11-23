using Backups.Interfaces;

namespace Backups.Client.Interfaces
{
    public interface IServerStorage : IStorage
    {
        string IpAddress { get; }
        int Port { get; }
    }
}