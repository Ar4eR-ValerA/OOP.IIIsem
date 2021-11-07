using System;

namespace Banks.Models
{
    public class DepositMoneyGap
    {
        public DepositMoneyGap(decimal from, decimal to, decimal percent)
        {
            From = from;
            To = to;
            Percent = percent;
            Id = Guid.NewGuid();
        }

        public DepositMoneyGap()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public decimal From { get; private set; }
        public decimal To { get; private set; }
        public decimal Percent { get; private set; }
        public override string ToString()
        {
            return $"{From}->{To}: {Percent}%";
        }

        public bool InMoneyGap(decimal money)
        {
            return From <= money && money <= To;
        }
    }
}