using System;
using Banks.Tools;

namespace Banks.Entities.Bills
{
    public abstract class BaseBill
    {
        private decimal _money;
        private decimal _dailyProfits;

        internal BaseBill(
            Bank bank,
            Client client,
            decimal money,
            decimal percent,
            decimal commission,
            decimal limit,
            DateTime endDate,
            decimal unreliableLimit,
            DateTime openDate,
            bool reliable)
        {
            Id = Guid.NewGuid();
            Bank = bank;
            Client = client;
            OpenDate = DateTime.Now;
            DailyProfits = 0;
            Limit = limit;
            Money = money;
            Percent = percent;
            Commission = commission;
            EndDate = endDate;
            UnreliableLimit = unreliableLimit;
            OpenDate = openDate;
            Reliable = reliable;
        }

        internal BaseBill()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Bank Bank { get; private set; }
        public Client Client { get; private set; }
        public DateTime OpenDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal UnreliableLimit { get; private set; }
        public bool Reliable { get; internal set; }

        public decimal DailyProfits
        {
            get => _dailyProfits;
            internal set
            {
                if (value < 0)
                {
                    throw new BanksException($"Daily profits must be non-negative.\nYour value: {value}");
                }

                _dailyProfits = value;
            }
        }

        public decimal Money
        {
            get => _money;
            internal set
            {
                if (value < Limit)
                {
                    throw new BanksException($"You have reached the limit.\nYour money: {Money}" +
                                             $"\nYou've tried withdraw: {value}" +
                                             $"\nYour limit: {Limit}");
                }

                _money = value;
            }
        }

        public decimal Percent { get; private set; }
        public decimal Commission { get; private set; }
        public decimal Limit { get; private set; }

        internal void ServiceDailyProfits()
        {
            if (Money <= 0) return;
            DailyProfits += (DailyProfits + Money) * Percent / (365 * 100);
        }

        internal Transaction ServicePercent(DateTime dateNow)
        {
            if (!IsNewMonth(dateNow)) return null;
            var transaction = new Transaction(Bank, this, _dailyProfits);
            DailyProfits = 0;

            return transaction;
        }

        internal Transaction ServiceCommission(DateTime dateNow)
        {
            if (!IsNewMonth(dateNow) || Money > 0) return null;

            var transaction = new Transaction(this, Bank, _dailyProfits);
            DailyProfits = 0;

            return transaction;
        }

        private bool IsNewMonth(DateTime dateNow) => dateNow.Day == 1;
    }
}