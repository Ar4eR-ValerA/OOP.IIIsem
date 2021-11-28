using System.Net;

namespace Backups.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
            Receiver.Receive(IPAddress.Parse("127.0.0.1"), 8888);
        }
    }
}