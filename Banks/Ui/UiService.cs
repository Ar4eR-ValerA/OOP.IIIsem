using System;
using Banks.Entities;
using Banks.Models.Builders;
using Banks.Ui.Tools;
using Spectre.Console;

namespace Banks.Ui
{
    public class UiService
    {
        private readonly CentralBank _centralBank;
        private readonly Executor _executor;
        private readonly Asker _asker;
        private ClientBuilder _clientBuilder;
        private Client _client;

        public UiService(CentralBank centralBank)
        {
            _executor = new Executor();
            _centralBank = centralBank;
            _asker = new Asker();
        }

        public void Run()
        {
            _clientBuilder = _executor.ExecuteCreateClientInfo();
            Guid clientId = _executor.ExecuteRegisterClient(_clientBuilder, _centralBank);
            _client = _centralBank.FindClient(clientId);

            _executor.ExecuteShowInfo(_clientBuilder, _centralBank);
            _executor.ExecuteRenderMainTable(_centralBank);

            Command[] commands =
            {
                new Command(
                    "open debit bill",
                    () => _executor.ExecuteOpenDebitBill(_client, _centralBank)),
                new Command(
                    "open deposit bill",
                    () => _executor.ExecuteOpenDepositBill(_client, _centralBank)),
                new Command(
                    "open credit bill",
                    () => _executor.ExecuteOpenCreditBill(_client, _centralBank)),
                new Command(
                    "show bank conditions",
                    () => _executor.ExecuteShowBank(_centralBank)),
                new Command(
                    "show bill",
                    () => _executor.ExecuteShowBill(_centralBank, _client.Id)),
                new Command(
                    "show transaction",
                    () => _executor.ExecuteShowTransaction(_centralBank)),
                new Command(
                    "show notification",
                    () => _executor.ExecuteShowNotification(_centralBank, _client.Id)),
                new Command(
                    "rewind time",
                    () => _executor.ExecuteRewindTime(_centralBank)),
                new Command(
                    "make transaction",
                    () => _executor.ExecuteMakeTransaction(_centralBank, _client.Id)),
                new Command(
                    "Enable notifications",
                    () => _executor.ExecuteEnableNotifications(_centralBank, _client.Id)),
                new Command(
                    "Forbid notifications",
                    () => _executor.ExecuteForbidNotifications(_centralBank, _client.Id)),
                new Command("exit"),
            };

            Command command = _asker.AskChoices("Enter command", commands);

            while (command.Title != "exit")
            {
                command.Action();

                AnsiConsole.Clear();

                _executor.ExecuteShowInfo(_clientBuilder, _centralBank);
                _executor.ExecuteRenderMainTable(_centralBank);
                command = _asker.AskChoices("Enter command", commands);
            }
        }
    }
}