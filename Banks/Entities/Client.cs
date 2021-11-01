using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        public Client(string name, string surname)
        {
            Name = name ?? throw new BanksException("Name is null");
            Surname = surname ?? throw new BanksException("Surname is null");
            Id = Guid.NewGuid();
        }

        public Client()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
    }
}