using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        public Bank(string name)
        {
            Name = name ?? throw new BanksException("Name is null");
            Id = Guid.NewGuid();
        }

        public Bank()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}