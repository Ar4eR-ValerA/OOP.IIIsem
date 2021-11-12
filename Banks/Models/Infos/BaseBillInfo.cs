using System;
using Banks.Entities.Bills;

namespace Banks.Models.Infos
{
    public abstract class BaseBillInfo
    {
        internal BaseBillInfo(Guid bankId, Guid clientId, decimal money)
        {
            BankId = bankId;
            ClientId = clientId;
            Money = money;
        }

        public Guid BankId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Money { get; set; }
        public decimal Percent { get; protected set; }
        public decimal UnreliableLimit { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public DateTime OpenDate { get; protected set; }
        public bool Reliable { get; protected set; }
        public decimal Limit { get; protected set; }
        public decimal Commission { get; protected set; }
        internal abstract BaseBill CreateBill();

        internal abstract void AddBankInfo(
            decimal percent,
            decimal limit,
            decimal commission,
            decimal unreliableLimit,
            DateTime openDate,
            DateTime endDate,
            bool reliable);
    }
}