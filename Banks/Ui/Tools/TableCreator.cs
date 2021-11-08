using System.Linq;
using Banks.Entities;
using Banks.Entities.Bills;
using Spectre.Console;

namespace Banks.Ui.Tools
{
    public class TableCreator
    {
        public (Table, Table, Table, Table) CreateMainTable()
        {
            var mainTable = new Table();
            var banksTable = new Table();
            var billsTable = new Table();
            var transactionsTable = new Table();

            mainTable.AddColumn("Banks");
            mainTable.AddColumn("Bills");
            mainTable.AddColumn("Transactions");
            mainTable.AddRow(banksTable, billsTable, transactionsTable);

            banksTable.AddColumn("Name:");
            banksTable.AddColumn("id:");
            banksTable.Border = TableBorder.None;

            billsTable.AddColumn("owner:");
            billsTable.AddColumn("Type:");
            billsTable.AddColumn("id:");
            billsTable.Border = TableBorder.None;

            transactionsTable.AddColumn("id:");
            transactionsTable.Border = TableBorder.None;

            return (mainTable, banksTable, billsTable, transactionsTable);
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
            var billTable = new Table
            {
                Title = new TableTitle($"{bill.GetType().ToString().Split(".").Last()} id: {bill.Id}"),
            };

            billTable.AddColumns(
                "Bank id",
                "Client id",
                "Open date",
                "End date",
                "Money",
                "Percent",
                "Commission",
                "Limit");

            return billTable;
        }

        public Table CreateTransactionPersonalTable(Transaction transaction)
        {
            var transactionTable = new Table
            {
                Title = new TableTitle($"id: {transaction.Id}"),
            };

            transactionTable.AddColumns(
                "Bill from",
                "Bill to",
                "Money");

            return transactionTable;
        }
    }
}