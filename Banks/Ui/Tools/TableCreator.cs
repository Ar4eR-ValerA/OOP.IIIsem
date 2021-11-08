using System.Linq;
using Banks.Entities;
using Banks.Entities.Bills;
using Banks.Models;
using Spectre.Console;

namespace Banks.Ui.Tools
{
    public class TableCreator
    {
        public (Table, Table, Table, Table, Table) CreateMainTable()
        {
            var mainTable = new Table();
            var banksTable = new Table();
            var billsTable = new Table();
            var transactionsTable = new Table();
            var notificationsTable = new Table();

            mainTable.AddColumn("Banks");
            mainTable.AddColumn("Bills");
            mainTable.AddColumn("Transactions");
            mainTable.AddColumn("Notifications");
            mainTable.AddRow(banksTable, billsTable, transactionsTable, notificationsTable);

            banksTable.AddColumn("Name:");
            banksTable.AddColumn("id:");
            banksTable.Border = TableBorder.None;

            billsTable.AddColumn("owner:");
            billsTable.AddColumn("Type:");
            billsTable.AddColumn("id:");
            billsTable.Border = TableBorder.None;

            transactionsTable.AddColumn("id:");
            transactionsTable.Border = TableBorder.None;

            notificationsTable.AddColumn("owner:");
            notificationsTable.AddColumn("id:");
            notificationsTable.Border = TableBorder.None;

            return (mainTable, banksTable, billsTable, transactionsTable, notificationsTable);
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

        public Table CreateNotificationPersonalTable(Notification notification)
        {
            var notificationTable = new Table
            {
                Title = new TableTitle($"id: {notification.Id}"),
            };

            notificationTable.AddColumns(
                "Client id",
                "Message");

            return notificationTable;
        }
    }
}