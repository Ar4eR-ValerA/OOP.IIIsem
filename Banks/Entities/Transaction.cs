using System;

namespace Banks.Entities
{
    public class Transaction
    {
        public Transaction(Guid from, Guid to, int amount)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
            Amount = amount;
        }

        public Transaction()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid From { get; private set; }
        public Guid To { get; private set; }
        public int Amount { get; private set; }
    }
}