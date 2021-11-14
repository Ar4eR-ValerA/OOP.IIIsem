using System;
using Banks.Models.Builders;

namespace Banks.Entities.Bills
{
    public class DebitBill : BaseBill
    {
        internal DebitBill(
            Bank bank,
            Client client,
            decimal money,
            decimal percent,
            DateTime endDate,
            decimal unreliableLimit,
            DateTime openDate,
            bool reliable)
            : base(
                bank,
                client,
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