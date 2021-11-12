using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class CreditBill : BaseBill
    {
        internal CreditBill(
            Guid bankId,
            Guid clientId,
            decimal money,
            decimal commission,
            decimal limit,
            DateTime endDate,
            decimal unreliableLimit,
            DateTime openDate,
            bool reliable)
            : base(
                bankId,
                clientId,
                money,
                0,
                commission,
                limit,
                endDate,
                unreliableLimit,
                openDate,
                reliable)
        {
        }

        internal CreditBill()
            : base()
        {
        }
    }
}