using System;
using Banks.Entities.Bills;
using Banks.Tools;

namespace Banks.Models.Infos
{
    public class DebitBillInfo : BaseBillInfo
    {
        public DebitBillInfo(Guid bankId, Guid clientId, decimal money)
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
            Limit = limit;
            Commission = commission;
            OpenDate = openDate;
            EndDate = endDate;
            UnreliableLimit = unreliableLimit;
            Reliable = reliable;
        }

        internal override BaseBill CreateBill()
        {
            return new DebitBill(this);
        }
    }
}