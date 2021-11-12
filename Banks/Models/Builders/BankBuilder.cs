using System.Collections.Generic;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Models.Builders
{
    public class BankBuilder
    {
        private string _name;
        private List<DepositMoneyGap> _depositMoneyGaps;

        public BankBuilder(
            string name,
            decimal debitPercent,
            decimal creditCommission,
            decimal limit,
            List<DepositMoneyGap> depositMoneyGaps,
            int billDurationYears,
            decimal unreliableLimit)
        {
            Name = name;
            DebitPercent = debitPercent;
            CreditCommission = creditCommission;
            DepositMoneyGaps = depositMoneyGaps;
            Limit = limit;
            BillDurationYears = billDurationYears;
            UnreliableLimit = unreliableLimit;
        }

        public string Name
        {
            get => _name;
            set => _name = value ?? throw new BanksException("Name is null");
        }

        public decimal DebitPercent { get; set; }
        public decimal CreditCommission { get; set; }
        public decimal Limit { get; set; }
        public decimal UnreliableLimit { get; set; }
        public int BillDurationYears { get; set; }

        public List<DepositMoneyGap> DepositMoneyGaps
        {
            get => _depositMoneyGaps;
            set => _depositMoneyGaps = value ?? throw new BanksException("Deposit Money Gaps is null");
        }

        internal Bank Build()
        {
            return new Bank(this);
        }
    }
}