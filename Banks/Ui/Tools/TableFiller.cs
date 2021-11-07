using System;
using System.Globalization;
using System.Linq;
using Banks.Entities;
using Banks.Entities.Bills;
using Banks.Models;
using Spectre.Console;

namespace Banks.Ui.Tools
{
    public class TableFiller
    {
        public void FillMainTable(Table banksTable, Table billsTable, CentralBank centralBank)
        {
            foreach (Bank bank in centralBank.Banks)
            {
                banksTable.AddRow(bank.Name, bank.Id.ToString());
            }

            foreach (BaseBill bill in centralBank.Bills)
            {
                billsTable.AddRow(bill.GetType().ToString().Split(".").Last(), bill.Id.ToString());
            }
        }

        public void FillBankPersonalTable(Table bankTable, Bank bank)
        {
            bankTable.AddRow(
                bank.DebitPercent.ToString(CultureInfo.InvariantCulture),
                bank.CreditCommission.ToString(CultureInfo.InvariantCulture),
                bank.Limit.ToString(CultureInfo.InvariantCulture),
                bank.BillDurationYears.ToString(CultureInfo.InvariantCulture),
                bank.UnreliableLimit.ToString(CultureInfo.InvariantCulture),
                "From->To: Percent%");

            foreach (DepositMoneyGap depositMoneyGap in bank.DepositMoneyGaps)
            {
                bankTable.AddRow(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    depositMoneyGap.ToString());
            }
        }

        public void FillBillPersonalTable(Table billTable, BaseBill bill)
        {
            billTable.AddRow(
                bill.BankId.ToString(),
                bill.ClientId.ToString(),
                bill.OpenDate.ToString(CultureInfo.InvariantCulture),
                bill.EndDate.ToString(CultureInfo.InvariantCulture),
                bill.Money.ToString(CultureInfo.InvariantCulture),
                bill.Percent.ToString(CultureInfo.InvariantCulture),
                bill.Commission.ToString(CultureInfo.InvariantCulture),
                bill.Limit.ToString(CultureInfo.InvariantCulture));
        }
    }
}