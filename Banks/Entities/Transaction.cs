using System;

namespace Banks.Entities
{
    public class Transaction
    {
        public Transaction(Guid from, Guid to)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
        }

        public Transaction()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid From { get; private set; }
        public Guid To { get; private set; }
    }
}