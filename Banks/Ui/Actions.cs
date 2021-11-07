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

            (mainTable, banksTable, billsTable) = _tableCreator.CreateMainTable();
            _tableFiller.FillMainTable(banksTable, billsTable, centralBank);

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

            Table bankTable = _tableCreator.CreateBankPersonalTable(centralBank.GetBank(bankId));
            _tableFiller.FillBankPersonalTable(bankTable, centralBank.GetBank(bankId));

            AnsiConsole.Write(bankTable);

            _asker.AskExit(string.Empty);
        }

        public void ShowBill(CentralBank centralBank, Guid billId)
        {
            AnsiConsole.Clear();

            Table billTable = _tableCreator.CreateBillPersonalTable(centralBank.GetBill(billId));
            _tableFiller.FillBillPersonalTable(billTable, centralBank.GetBill(billId));

            AnsiConsole.Write(billTable);

            _asker.AskExit(string.Empty);
        }

        public void RewindTime(CentralBank centralBank, int monthAmount)
        {
            DateTime targetDate = centralBank.DateNow;
            targetDate = targetDate.AddMonths(monthAmount);

            centralBank.RewindTime(targetDate);
        }
    }
}