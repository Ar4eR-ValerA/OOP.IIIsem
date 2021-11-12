using System;
using System.Collections.Generic;
using Banks.Contexts;
using Banks.Entities;
using Banks.Models;
using Banks.Models.Builders;
using Banks.Ui;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var centralBank = new CentralBank(new CentralBankContext("file.db"), DateTime.Now);

            var bankInfo1 = new BankBuilder(
                "Sber",
                3,
                7,
                -800000,
                new List<DepositMoneyGap>
                {
                    new DepositMoneyGap(100, 200, 2),
                    new DepositMoneyGap(200, 20000, 5),
                },
                5,
                200000);

            var bankInfo2 = new BankBuilder(
                "Tinkoff",
                5,
                9,
                -100000,
                new List<DepositMoneyGap>
                {
                    new DepositMoneyGap(100, 2000, 3),
                    new DepositMoneyGap(2000, 25000, 8),
                },
                8,
                4000);

            var bankInfo3 = new BankBuilder(
                "Alpha",
                2,
                3,
                -15000,
                new List<DepositMoneyGap>
                {
                    new DepositMoneyGap(100, 20000, 5),
                    new DepositMoneyGap(20000, 800000, 10),
                },
                5,
                1000);

            Guid bankId1 = centralBank.RegisterBank(bankInfo1);
            centralBank.RegisterBank(bankInfo2);
            centralBank.RegisterBank(bankInfo3);

            var clientInfo = new ClientBuilder("Max", "Shevchenko", "Dom 21", 1111);
            Guid clientId = centralBank.RegisterClient(clientInfo);
            centralBank.OpenBill(new DebitBillBuilder(bankId1, clientId, 1000));

            var ui = new UiService(centralBank);
            ui.Run();
        }
    }
}