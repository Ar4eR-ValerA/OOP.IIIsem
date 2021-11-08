using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities;
using Banks.Models.Infos;
using Banks.Tools;

namespace Banks.Ui.Tools
{
    public class Executor
    {
        private readonly Inputter _inputter;
        private readonly Asker _asker;
        private readonly Actions _actions;

        public Executor()
        {
            _inputter = new Inputter();
            _actions = new Actions();
            _asker = new Asker();
        }

        public ClientInfo ExecuteCreateClientInfo()
        {
            try
            {
                return _actions.CreateClientInfo(_inputter.InputName(), _inputter.InputSurname());
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
                return _actions.CreateClientInfo("Error", "Error");
            }
        }

        public void ExecuteRenderMainTable(CentralBank centralBank)
        {
            try
            {
                _actions.RenderMainTable(centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteShowInfo(ClientInfo clientInfo, CentralBank centralBank)
        {
            try
            {
                _actions.ShowInfo(clientInfo, centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public Guid ExecuteRegisterClient(ClientInfo clientInfo, CentralBank centralBank)
        {
            try
            {
                return _actions.RegisterClient(clientInfo, centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
                return Guid.Empty;
            }
        }

        public void ExecuteOpenDebitBill(Guid clientId, CentralBank centralBank)
        {
            try
            {
                _actions.OpenDebitBill(
                    new DebitBillInfo(
                        _inputter.InputBankId(centralBank.Banks.Select(bank => bank.Id)),
                        clientId,
                        _inputter.InputMoney()),
                    centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteOpenDepositBill(Guid clientId, CentralBank centralBank)
        {
            try
            {
                _actions.OpenDepositBill(
                    new DepositBillInfo(
                        _inputter.InputBankId(centralBank.Banks.Select(bank => bank.Id)),
                        clientId,
                        _inputter.InputMoney()),
                    centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteOpenCreditBill(Guid clientId, CentralBank centralBank)
        {
            try
            {
                _actions.OpenCreditBill(
                    new CreditBillInfo(
                        _inputter.InputBankId(centralBank.Banks.Select(bank => bank.Id)),
                        clientId,
                        _inputter.InputMoney()),
                    centralBank);
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteShowBank(CentralBank centralBank)
        {
            try
            {
                _actions.ShowBank(
                    centralBank,
                    _inputter.InputBankId(centralBank.Banks.Select(bank => bank.Id)));
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteShowTransaction(CentralBank centralBank)
        {
            try
            {
                _actions.ShowTransaction(
                    centralBank,
                    _inputter.InputTransactionId(centralBank.Transactions.Select(transaction => transaction.Id)));
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteShowBill(CentralBank centralBank)
        {
            try
            {
                _actions.ShowBill(
                    centralBank,
                    _inputter.InputBillId(centralBank.Bills.Select(bill => bill.Id)));
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteRewindTime(CentralBank centralBank)
        {
            try
            {
                _actions.RewindTime(
                    centralBank,
                    _inputter.InputMonthAmount());
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }

        public void ExecuteMakeTransaction(CentralBank centralBank, Guid clientId)
        {
            try
            {
                _actions.MakeTransaction(
                    centralBank,
                    _inputter.InputBillFromId(centralBank.Bills
                        .Where(bill => bill.ClientId == clientId)
                        .Select(bill => bill.Id)),
                    _inputter.InputBillToId(centralBank.Bills.Select(bill => bill.Id)),
                    _inputter.InputMoney());
            }
            catch (BanksException exception)
            {
                _asker.AskExit(exception.Message);
            }
        }
    }
}