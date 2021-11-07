using System.Linq;
using Banks.Entities;
using Banks.Entities.Bills;
using Spectre.Console;

namespace Banks.Ui.Tools
{
    public class TableCreator
    {
        public (Table, Table, Table) CreateMainTable()
        {
            var mainTable = new Table();
            var banksTable = new Table();
            var billsTable = new Table();

            mainTable.AddColumn("Banks");
            mainTable.AddColumn("Bills");
            mainTable.AddRow(banksTable, billsTable);

            banksTable.AddColumn("Name:");
            banksTable.AddColumn("id:");
            banksTable.Border = TableBorder.None;

            billsTable.AddColumn("Type:");
            billsTable.AddColumn("id:");
            billsTable.Border = TableBorder.None;

            return (mainTable, banksTable, billsTable);
        }

        public Table CreateBankPersonalTable(Bank bank)
        {
            var bankTable = new Table
            {
                Title = new TableTitle($"{bank.Name} id: {bank.Id}"),
            };

            bankTable.AddColumns(
                "Debit percent",
                "Credit commission",
                "Credit Limit",
                "Bill duration (years)",
                "Unreliable limit",
                "Deposit money gaps");

            return bankTable;
        }

        public Table CreateBillPersonalTable(BaseBill bill)
        {
            var bankTable = new Table
            {
                Title = new TableTitle($"{bill.GetType().ToString().Split(".").Last()} id: {bill.Id}"),
            };

            bankTable.AddColumns(
                "Bank id",
                "Client id",
                "Open date",
                "End date",
                "Money",
                "Percent",
                "Commission",
                "Limit");

            return bankTable;
        }
    }
}