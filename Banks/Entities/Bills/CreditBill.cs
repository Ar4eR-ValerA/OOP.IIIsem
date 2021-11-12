using System;
using Banks.Models.Infos;

namespace Banks.Entities.Bills
{
    public class CreditBill : BaseBill
    {
        internal CreditBill(CreditBillInfo billInfo)
            : base(
                billInfo.BankId,
                billInfo.ClientId,
                billInfo.Money,
                0,
                billInfo.Commission,
                billInfo.Limit,
                billInfo.EndDate,
                billInfo.UnreliableLimit,
                billInfo.OpenDate,
                billInfo.Reliable)
        {
        }

        internal CreditBill()
            : base()
        {
        }
    }
}