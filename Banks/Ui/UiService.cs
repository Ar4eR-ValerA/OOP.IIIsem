using System;
using Banks.Entities;
using Banks.Models.Infos;
using Banks.Ui.Tools;
using Spectre.Console;

namespace Banks.Ui
{
    public class UiService
    {
        private readonly CentralBank _centralBank;
        private readonly Executor _executor;
        private readonly Asker _asker;
        private ClientInfo _clientInfo;
        private Guid _clientId;

        public UiService(CentralBank centralBank)
        {
            _executor = new Executor();
            _centralBank = centralBank;
            _asker = new Asker();
        }

        public void Run()
        {
            _clientInfo = _executor.ExecuteCreateClientInfo();
            _clientId = _executor.ExecuteRegisterClient(_clientInfo, _centralBank);

            _executor.ExecuteShowInfo(_clientInfo, _centralBank);
            _executor.ExecuteRenderMainTable(_centralBank);

            Command[] commands =
            {
                new Command(
                    "open debit bill",
                    () => _executor.ExecuteOpenDebitBill(_clientId, _centralBank)),
                new Command(
                    "open deposit bill",
                    () => _executor.ExecuteOpenDepositBill(_clientId, _centralBank)),
                new Command(
                    "open credit bill",
                    () => _executor.ExecuteOpenCreditBill(_clientId, _centralBank)),
                new Command(
                    "show bank conditions",
                    () => _executor.ExecuteShowBank(_centralBank)),
                new Command(
                    "show bill",
                    () => _executor.ExecuteShowBill(_centralBank)),
                new Command(
                    "show transaction",
                    () => _executor.ExecuteShowTransaction(_centralBank)),
                new Command(
                    "rewind time",
                    () => _executor.ExecuteRewindTime(_centralBank)),
                new Command(
                    "make transaction",
                    () => _executor.ExecuteMakeTransaction(_centralBank, _clientId)),
                new Command("exit"),
            };

            Command command = _asker.AskChoices("Enter command", commands);

            while (command.Title != "exit")
            {
                command.Action();

                AnsiConsole.Clear();

                _executor.ExecuteShowInfo(_clientInfo, _centralBank);
                _executor.ExecuteRenderMainTable(_centralBank);
                command = _asker.AskChoices("Enter command", commands);
            }
        }
    }
}