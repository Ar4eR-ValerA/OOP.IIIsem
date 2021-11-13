using System;

namespace Banks.Entities.Bills
{
    public class CreditBill : BaseBill
    {
        internal CreditBill(
            Bank bank,
            Client client,
            decimal money,
            decimal commission,
            decimal limit,
            DateTime endDate,
            decimal unreliableLimit,
            DateTime openDate,
            bool reliable)
            : base(
                bank,
                client,
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