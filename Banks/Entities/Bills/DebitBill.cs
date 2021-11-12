using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class DebitBill : BaseBill
    {
        internal DebitBill(DebitBillBuilder billBuilder)
            : base(
                billBuilder.BankId,
                billBuilder.ClientId,
                billBuilder.Money,
                billBuilder.Percent,
                0,
                0,
                billBuilder.EndDate,
                billBuilder.UnreliableLimit,
                billBuilder.OpenDate,
                billBuilder.Reliable)
        {
        }

        internal DebitBill()
            : base()
        {
        }
    }
}