using System;
using Banks.Models.Infos;

namespace Banks.Entities.Bills
{
    public class DepositBill : BaseBill
    {
        internal DepositBill(DepositBillInfo billInfo)
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

        internal DepositBill()
        {
        }
    }
}