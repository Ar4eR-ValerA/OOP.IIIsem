using System;

namespace Banks.Entities
{
    public class Transaction
    {
        public Transaction(Guid from, Guid to, decimal money)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
            Money = money;
            Valid = true;
        }

        public Transaction()
        {
            Id = Guid.NewGuid();
            Valid = true;
        }

        public Guid Id { get; private set; }
        public Guid From { get; private set; }
        public Guid To { get; private set; }
        public decimal Money { get; private set; }
        public bool Valid { get; internal set; }
    }
}