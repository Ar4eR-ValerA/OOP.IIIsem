using System;
using System.Collections.Generic;
using Banks.Contexts;
using Banks.Entities;
using Banks.Models;
using Banks.Models.Builders;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Banks.Tests
{
    public class Tests
    {
        private CentralBank _centralBank;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CentralBankContext>();

            _centralBank = new CentralBank(
                new CentralBankContext(optionsBuilder.UseInMemoryDatabase("file.db").Options),
                DateTime.Now);
        }

        [Test]
        public void ChangeBankInfo_BankInfoChanged()
        {
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
            Guid bankId = _centralBank.RegisterBank(bankInfo1);

            bankInfo1.DebitPercent = 1;
            bankInfo1.Limit = -100;
            bankInfo1.CreditCommission = 2;
            bankInfo1.DepositMoneyGaps.Clear();
            bankInfo1.DepositMoneyGaps.AddRange(new[]
            {
                new DepositMoneyGap(100, 2000, 2),
                new DepositMoneyGap(2000, 20000, 5),
            });

            _centralBank.ChangeBankInfo(bankId, bankInfo1);
            Bank bank = _centralBank.FindBank(bankId);
            Assert.AreEqual(bankInfo1.DebitPercent, bank.DebitPercent);
            Assert.AreEqual(bankInfo1.Limit, bank.Limit);
            Assert.AreEqual(bankInfo1.CreditCommission, bank.CreditCommission);
            CollectionAssert.AreEqual(bankInfo1.DepositMoneyGaps, bank.DepositMoneyGaps);
        }

        [Test]
        public void OpenBills_BillsOpened()
        {
            var bankInfo = new BankBuilder(
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
            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo = new ClientBuilder("Valera", "Shevchenko");
            Guid clientId = _centralBank.RegisterClient(clientInfo);
            Client client = _centralBank.FindClient(clientId);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client, 101));
            Guid billId2 = _centralBank.OpenBill(new CreditBillBuilder(bank, client, 102));
            Guid billId3 = _centralBank.OpenBill(new DepositBillBuilder(bank, client, 103));

            Assert.AreEqual(101, _centralBank.FindBill(billId1).Money);
            Assert.AreEqual(102, _centralBank.FindBill(billId2).Money);
            Assert.AreEqual(103, _centralBank.FindBill(billId3).Money);
        }

        [Test]
        public void MakeTransaction_TransactionMade()
        {
            var bankInfo = new BankBuilder(
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

            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo1 = new ClientBuilder("Valera", "Shevchenko");
            var clientInfo2 = new ClientBuilder("Max", "Shevchenko");

            Guid clientId1 = _centralBank.RegisterClient(clientInfo1);
            Client client1 = _centralBank.FindClient(clientId1);

            Guid clientId2 = _centralBank.RegisterClient(clientInfo2);
            Client client2 = _centralBank.FindClient(clientId2);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client1, 100));
            Guid billId2 = _centralBank.OpenBill(new DebitBillBuilder(bank, client2, 100));

            _centralBank.MakeTransaction(billId1, billId2, 50);

            Assert.AreEqual(50, _centralBank.FindBill(billId1).Money);
            Assert.AreEqual(150, _centralBank.FindBill(billId2).Money);
        }

        [Test]
        public void CancelTransaction_TransactionCanceled()
        {
            var bankInfo = new BankBuilder(
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
            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo1 = new ClientBuilder("Valera", "Shevchenko");
            var clientInfo2 = new ClientBuilder("Max", "Shevchenko");

            Guid clientId1 = _centralBank.RegisterClient(clientInfo1);
            Guid clientId2 = _centralBank.RegisterClient(clientInfo2);

            Client client1 = _centralBank.FindClient(clientId1);
            Client client2 = _centralBank.FindClient(clientId2);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client1, 100));
            Guid billId2 = _centralBank.OpenBill(new DebitBillBuilder(bank, client2, 100));

            Guid transactionId = _centralBank.MakeTransaction(billId1, billId2, 50);
            _centralBank.CancelTransaction(transactionId);

            Assert.AreEqual(100, _centralBank.FindBill(billId1).Money);
            Assert.AreEqual(100, _centralBank.FindBill(billId2).Money);
        }

        [Test]
        public void AddClientInfo_ClientInfoAdded()
        {
            var bankInfo = new BankBuilder(
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
                100);
            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo1 = new ClientBuilder("Valera", "Shevchenko");
            var clientInfo2 = new ClientBuilder("Max", "Shevchenko", "Dom 21", 1111);

            Guid clientId1 = _centralBank.RegisterClient(clientInfo1);
            Guid clientId2 = _centralBank.RegisterClient(clientInfo2);

            Client client1 = _centralBank.FindClient(clientId1);
            Client client2 = _centralBank.FindClient(clientId2);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client1, 200));
            Guid billId2 = _centralBank.OpenBill(new DebitBillBuilder(bank, client2, 200));

            Assert.Catch<BanksException>(() =>
            {
                _centralBank.MakeTransaction(billId1, billId2, 150);
            });

            _centralBank.AddClientInfo(clientId1, "Dom 3", 2222);
            _centralBank.MakeTransaction(billId1, billId2, 150);

            Assert.AreEqual(50, _centralBank.FindBill(billId1).Money);
            Assert.AreEqual(350, _centralBank.FindBill(billId2).Money);
            Assert.AreEqual(true, _centralBank.FindClient(clientId1).Reliable);
            Assert.AreEqual(true, _centralBank.FindClient(clientId2).Reliable);
            Assert.AreEqual(true, _centralBank.FindBill(billId1).Reliable);
            Assert.AreEqual(true, _centralBank.FindBill(billId2).Reliable);
        }

        [Test]
        public void MakeInvalidTransaction_ThrowException()
        {
            var bankInfo = new BankBuilder(
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
            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo1 = new ClientBuilder("Valera", "Shevchenko");
            var clientInfo2 = new ClientBuilder("Max", "Shevchenko");

            Guid clientId1 = _centralBank.RegisterClient(clientInfo1);
            Guid clientId2 = _centralBank.RegisterClient(clientInfo2);

            Client client1 = _centralBank.FindClient(clientId1);
            Client client2 = _centralBank.FindClient(clientId2);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client1, 100));
            Guid billId2 = _centralBank.OpenBill(new DebitBillBuilder(bank, client2, 100));

            Assert.Catch<BanksException>(() =>
            {
                _centralBank.MakeTransaction(billId1, billId2, 150);
            });
        }

        [Test]
        public void EnableNotifications_NotificationsSending()
        {
            var bankInfo = new BankBuilder(
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
            Guid bankId = _centralBank.RegisterBank(bankInfo);
            Bank bank = _centralBank.FindBank(bankId);

            var clientInfo = new ClientBuilder("Valera", "Shevchenko");
            Guid clientId = _centralBank.RegisterClient(clientInfo);
            Client client = _centralBank.FindClient(clientId);

            Guid billId1 = _centralBank.OpenBill(new DebitBillBuilder(bank, client, 100));

            _centralBank.EnableNotification(clientId);

            bankInfo.CreditCommission = 9;
            _centralBank.ChangeBankInfo(bankId, bankInfo);

            Assert.AreEqual(1, _centralBank.Notifications.Count);

            _centralBank.ForbidNotification(clientId);
            bankInfo.CreditCommission = 2;

            Assert.AreEqual(1, _centralBank.Notifications.Count);
        }
    }
}