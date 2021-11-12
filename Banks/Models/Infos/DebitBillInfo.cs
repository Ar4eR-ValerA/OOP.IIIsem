using System;
using Banks.Tools;

namespace Banks.Models.Infos
{
    public class DebitBillInfo
    {
        public DebitBillInfo(Guid bankId, Guid clientId, decimal money)
        {
            BankId = bankId;
            ClientId = clientId;
            Money = money;
        }

        public Guid BankId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Money { get; set; }
        public decimal Percent { get; private set; }
        public decimal UnreliableLimit { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime OpenDate { get; private set; }
        public bool Reliable { get; private set; }

        internal void AddBankInfo(
            decimal percent,
            decimal unreliableLimit,
            DateTime openDate,
            DateTime endDate,
            bool reliable)
        {
            if (percent < 0)
            {
                throw new BanksException($"Percent must be non-negative. Your percent: {percent}");
            }

            if (openDate > endDate)
            {
                throw new BanksException($"End date must be later than open date." +
                                         $"\nOpen date: {openDate}" +
                                         $"\nOpen date: {endDate}");
            }

            Percent = percent;
            OpenDate = openDate;
            EndDate = endDate;
            UnreliableLimit = unreliableLimit;
            Reliable = reliable;
        }
    }
}