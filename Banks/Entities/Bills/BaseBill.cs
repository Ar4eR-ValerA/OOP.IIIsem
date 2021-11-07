using System;
using Banks.Tools;

namespace Banks.Entities.Bills
{
    public abstract class BaseBill
    {
        private decimal _money;

        public BaseBill(
            Guid bankId,
            Guid clientId,
            decimal money,
            decimal percent,
            decimal commission,
            decimal limit,
            DateTime endDate)
        {
            Id = Guid.NewGuid();
            BankId = bankId;
            ClientId = clientId;
            OpenDate = DateTime.Now;
            DailyProfits = 0;
            Limit = limit;
            Money = money;
            Percent = percent;
            Commission = commission;
            EndDate = endDate;
        }

        public BaseBill()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; internal set; }
        public Guid BankId { get; internal set; }
        public Guid ClientId { get; internal set; }
        public DateTime OpenDate { get; internal set; }
        public DateTime EndDate { get; internal set; }
        public bool Reliable { get; internal set; }
        public decimal UnreliableLimit { get; internal set; }
        public decimal DailyProfits { get; internal set; }

        public decimal Money
        {
            get => _money;
            internal set
            {
                if (value < Limit)
                {
                    throw new BanksException($"You have reached the limit.\nYour money: {Money}\nYour limit: {Limit}");
                }

                _money = value;
            }
        }

        public decimal Percent { get; internal set; }
        public decimal Commission { get; internal set; }
        public decimal Limit { get; internal set; }
    }
}