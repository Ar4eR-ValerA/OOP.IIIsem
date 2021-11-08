using System;
using Banks.Entities;
using Banks.Models.Infos;
using Banks.Ui.Tools;
using Spectre.Console;

namespace Banks.Ui
{
    public class Actions
    {
        private readonly Asker _asker;
        private readonly TableCreator _tableCreator;
        private readonly TableFiller _tableFiller;

        public Actions()
        {
            _tableFiller = new TableFiller();
            _tableCreator = new TableCreator();
            _asker = new Asker();
        }

        public void RenderMainTable(CentralBank centralBank)
        {
            Table mainTable;
            Table banksTable;
            Table billsTable;
            Table transactionsTable;
            Table notificationsTable;

            (mainTable, banksTable, billsTable, transactionsTable, notificationsTable) =
                _tableCreator.CreateMainTable();

            _tableFiller.FillMainTable(banksTable, billsTable, transactionsTable, notificationsTable, centralBank);

            AnsiConsole.Write(mainTable);
        }

        public ClientInfo CreateClientInfo(string name, string surname)
        {
            return new ClientInfo(name, surname);
        }

        public void ShowInfo(ClientInfo clientInfo, CentralBank centralBank)
        {
            AnsiConsole.Write($"{clientInfo.Name} {clientInfo.Surname}: {centralBank.DateNow}\n");
        }

        public Guid RegisterClient(ClientInfo clientInfo, CentralBank centralBank)
        {
            return centralBank.RegisterClient(clientInfo);
        }

        public void OpenDebitBill(DebitBillInfo billInfo, CentralBank centralBank)
        {
            centralBank.OpenBill(billInfo);
        }

        public void OpenDepositBill(DepositBillInfo billInfo, CentralBank centralBank)
        {
            centralBank.OpenBill(billInfo);
        }

        public void OpenCreditBill(CreditBillInfo billInfo, CentralBank centralBank)
        {
            centralBank.OpenBill(billInfo);
        }

        public void ShowBank(CentralBank centralBank, Guid bankId)
        {
            AnsiConsole.Clear();

            Table bankTable = _tableCreator.CreateBankPersonalTable(centralBank.FindBank(bankId));
            _tableFiller.FillBankPersonalTable(bankTable, centralBank.FindBank(bankId));

            AnsiConsole.Write(bankTable);

            _asker.AskExit(string.Empty);
        }

        public void ShowBill(CentralBank centralBank, Guid billId)
        {
            AnsiConsole.Clear();

            Table billTable = _tableCreator.CreateBillPersonalTable(centralBank.FindBill(billId));
            _tableFiller.FillBillPersonalTable(billTable, centralBank.FindBill(billId));

            AnsiConsole.Write(billTable);

            _asker.AskExit(string.Empty);
        }

        public void ShowTransaction(CentralBank centralBank, Guid transactionId)
        {
            AnsiConsole.Clear();

            Table transactionTable =
                _tableCreator.CreateTransactionPersonalTable(centralBank.FindTransaction(transactionId));
            _tableFiller.FillTransactionPersonalTable(transactionTable, centralBank.FindTransaction(transactionId));

            AnsiConsole.Write(transactionTable);

            _asker.AskExit(string.Empty);
        }

        public void ShowNotification(CentralBank centralBank, Guid notificationId)
        {
            AnsiConsole.Clear();

            Table notificationTable =
                _tableCreator.CreateNotificationPersonalTable(centralBank.FindNotification(notificationId));
            _tableFiller.FillNotificationPersonalTable(notificationTable, centralBank.FindNotification(notificationId));

            AnsiConsole.Write(notificationTable);

            _asker.AskExit(string.Empty);
        }

        public void RewindTime(CentralBank centralBank, int monthAmount)
        {
            DateTime targetDate = centralBank.DateNow;
            targetDate = targetDate.AddMonths(monthAmount);

            centralBank.RewindTime(targetDate);
        }

        public void MakeTransaction(CentralBank centralBank, Guid billFrom, Guid billTo, decimal money)
        {
            centralBank.MakeTransaction(billFrom, billTo, money);
        }

        public void EnableNotifications(CentralBank centralBank, Guid clientId)
        {
            centralBank.EnableNotification(clientId);
        }

        public void ForbidNotifications(CentralBank centralBank, Guid clientId)
        {
            centralBank.EnableNotification(clientId);
        }
    }
}