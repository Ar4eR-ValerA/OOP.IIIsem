using System;

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
        public decimal UnreliableLimit { get; internal set; }
        public decimal Limit { get; internal set; }
        public decimal Commission { get; internal set; }
        public DateTime EndDate { get; internal set; }
    }
}