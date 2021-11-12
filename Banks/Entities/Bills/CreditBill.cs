using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class CreditBill : BaseBill
    {
        internal CreditBill(CreditBillBuilder billBuilder)
            : base(
                billBuilder.BankId,
                billBuilder.ClientId,
                billBuilder.Money,
                0,
                billBuilder.Commission,
                billBuilder.Limit,
                billBuilder.EndDate,
                billBuilder.UnreliableLimit,
                billBuilder.OpenDate,
                billBuilder.Reliable)
        {
        }

        internal CreditBill()
            : base()
        {
        }
    }
}