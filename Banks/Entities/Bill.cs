using System;

namespace Banks.Entities
{
    public class Bill
    {
        internal Bill(Guid bankId, Guid clientId, decimal money)
        {
            BankId = bankId;
            ClientId = clientId;
            Money = money;
            Id = Guid.NewGuid();
        }

        internal Bill()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid BankId { get; private set; }
        public Guid ClientId { get; private set; }
        public decimal Money { get; private set; }
    }
}