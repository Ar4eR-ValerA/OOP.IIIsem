using System;
using Banks.Entities.Bills;
using Banks.Tools;

namespace Banks.Entities
{
    public class Transaction
    {
        public Transaction(BaseBill from, BaseBill to, decimal money)
        {
            Checks.MakeTransactionChecks(from, to, money);

            Id = Guid.NewGuid();
            From = from.Id;
            To = to.Id;
            Money = money;
            Valid = true;

            from.Money -= money;
            to.Money += money;
        }

        public Transaction(Bank from, BaseBill to, decimal money)
        {
            Checks.MakeBankTransactionChecks(from, to);

            Id = Guid.NewGuid();
            From = from.Id;
            To = to.Id;
            Money = money;
            Valid = true;

            to.Money += money;
        }

        public Transaction(BaseBill from, Bank to, decimal money)
        {
            Checks.MakeBankTransactionChecks(from, to);

            Id = Guid.NewGuid();
            From = from.Id;
            To = to.Id;
            Money = money;
            Valid = true;

            from.Money -= money;
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

        internal void Cancel()
        {
            Valid = false;
        }
    }
}