using System;
using Banks.Tools;

namespace Banks.Models.Infos
{
    public class CreditBillInfo
    {
        public CreditBillInfo(Guid bankId, Guid clientId, decimal money)
        {
            BankId = bankId;
            ClientId = clientId;
            Money = money;
        }

        public Guid BankId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Money { get; set; }
        public decimal UnreliableLimit { get; private set; }
        public decimal Limit { get; private set; }
        public decimal Commission { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateTime OpenDate { get; private set; }
        public bool Reliable { get; private set; }

        internal void AddBankInfo(
            decimal limit,
            decimal commission,
            decimal unreliableLimit,
            DateTime openDate,
            DateTime endDate,
            bool reliable)
        {
            if (commission < 0)
            {
                throw new BanksException($"Commission must be non-negative. Your percent: {commission}");
            }

            if (openDate > endDate)
            {
                throw new BanksException($"End date must be later than open date." +
                                         $"\nOpen date: {openDate}" +
                                         $"\nOpen date: {endDate}");
            }

            Limit = limit;
            Commission = commission;
            OpenDate = openDate;
            EndDate = endDate;
            UnreliableLimit = unreliableLimit;
            Reliable = reliable;
        }
    }
}