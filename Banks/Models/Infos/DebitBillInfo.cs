using System;

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
        public decimal Percent { get; internal set; }
        public decimal UnreliableLimit { get; internal set; }
        public DateTime EndDate { get; internal set; }
    }
}