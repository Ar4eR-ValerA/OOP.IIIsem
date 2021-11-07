using System;
using Banks.Models.Infos;

namespace Banks.Entities.Bills
{
    public class DebitBill : BaseBill
    {
        internal DebitBill(DebitBillInfo billInfo)
            : base(billInfo.BankId, billInfo.ClientId, billInfo.Money, billInfo.Percent, 0, 0, billInfo.EndDate)
        {
            OpenDate = DateTime.Now;
            DailyProfits = 0;
            Id = Guid.NewGuid();
        }

        internal DebitBill()
            : base()
        {
        }
    }
}