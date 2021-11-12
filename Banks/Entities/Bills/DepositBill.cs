using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class DepositBill : BaseBill
    {
        internal DepositBill(DepositBillBuilder billBuilder)
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

        internal DepositBill()
        {
        }
    }
}