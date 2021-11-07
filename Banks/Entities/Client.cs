using System;
using Banks.Models.Infos;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        public Client(ClientInfo clientInfo)
        {
            if (clientInfo is null)
            {
                throw new BanksException("Client's info is null");
            }

            Name = clientInfo.Name;
            Surname = clientInfo.Surname;
            Id = Guid.NewGuid();
            EnableNotification = false;
            Address = clientInfo.Address;
            Passport = clientInfo.Passport;

            if (Address is null || Passport == 0)
            {
                Reliable = false;
            }
            else
            {
                Reliable = true;
            }
        }

        internal Client()
        {
            Id = Guid.NewGuid();
            Reliable = false;
        }

        public bool Reliable { get; internal set; }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Address { get; internal set; }
        public int Passport { get; internal set; }
        public bool EnableNotification { get; internal set; }
    }
}