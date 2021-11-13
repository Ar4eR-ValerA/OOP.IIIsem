using System;
using Banks.Entities;
using Banks.Entities.Bills;

namespace Banks.Models.Builders
{
    public abstract class BaseBillBuilder
    {
        internal BaseBillBuilder(Bank bank, Client client, decimal money)
        {
            Bank = bank;
            Client = client;
            Money = money;
        }

        public Bank Bank { get; set; }
        public Client Client { get; set; }
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