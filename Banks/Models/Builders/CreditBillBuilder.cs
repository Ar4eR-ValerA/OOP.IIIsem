using System;
using Banks.Entities.Bills;
using Banks.Tools;

namespace Banks.Models.Builders
{
    public class CreditBillBuilder : BaseBillBuilder
    {
        public CreditBillBuilder(Guid bankId, Guid clientId, decimal money)
            : base(bankId, clientId, money)
        {
            BankId = bankId;
            ClientId = clientId;
            Money = money;
        }

        internal override void AddBankInfo(
            decimal percent,
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

            Percent = 0;
            Limit = limit;
            Commission = commission;
            OpenDate = openDate;
            EndDate = endDate;
            UnreliableLimit = unreliableLimit;
            Reliable = reliable;
        }

        internal override BaseBill CreateBill()
        {
            return new CreditBill(
                BankId,
                ClientId,
                Money,
                Commission,
                Limit,
                EndDate,
                UnreliableLimit,
                OpenDate,
                Reliable);
        }
    }
}