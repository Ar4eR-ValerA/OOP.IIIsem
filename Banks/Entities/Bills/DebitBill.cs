using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class DebitBill : BaseBill
    {
        internal DebitBill(
            Guid bankId,
            Guid clientId,
            decimal money,
            decimal percent,
            DateTime endDate,
            decimal unreliableLimit,
            DateTime openDate,
            bool reliable)
            : base(
                bankId,
                clientId,
                money,
                percent,
                0,
                0,
                endDate,
                unreliableLimit,
                openDate,
                reliable)
        {
        }

        internal DebitBill()
            : base()
        {
        }
    }
}