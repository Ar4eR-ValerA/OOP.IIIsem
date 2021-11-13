using System;
using Banks.Models.Builders;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        public Client(string name, string surname, string address, int passport)
        {
            Name = name ?? throw new BanksException("Name is null");
            Surname = surname ?? throw new BanksException("Surname is null");
            Id = Guid.NewGuid();
            EnableNotification = false;
            Address = address;
            Passport = passport;
        }

        internal Client()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Address { get; private set; }
        public int Passport { get; private set; }

        public bool Reliable => Address is not null && Passport != 0;

        public bool EnableNotification { get; internal set; }

        internal void AddClientInfo(string address, int passport)
        {
            Address = address ?? throw new BanksException("Address is null");
            Passport = passport;
        }
    }
}