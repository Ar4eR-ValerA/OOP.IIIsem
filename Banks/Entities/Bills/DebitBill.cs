using System;
using Banks.Models.Infos;

namespace Banks.Entities.Bills
{
    public class DebitBill : BaseBill
    {
        internal DebitBill(DebitBillInfo billInfo)
            : base(
                billInfo.BankId,
                billInfo.ClientId,
                billInfo.Money,
                billInfo.Percent,
                0,
                0,
                billInfo.EndDate,
                billInfo.UnreliableLimit,
                billInfo.OpenDate,
                billInfo.Reliable)
        {
        }

        internal DebitBill()
            : base()
        {
        }
    }
}