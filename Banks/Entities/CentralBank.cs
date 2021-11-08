using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Contexts;
using Banks.Entities.Bills;
using Banks.Models;
using Banks.Models.Infos;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        public CentralBank(CentralBankContext centralBankContext, DateTime dateNow)
        {
            CentralBankContext = centralBankContext;
            DateNow = dateNow;
        }

        public DateTime DateNow { get; private set; }
        public IReadOnlyList<Bank> Banks => CentralBankContext.Banks.ToList();
        public IReadOnlyList<Client> Clients => CentralBankContext.Clients.ToList();
        public IReadOnlyList<BaseBill> Bills => CentralBankContext.Bills.ToList();
        public IReadOnlyList<Transaction> Transactions => CentralBankContext.Transactions.ToList();
        public IReadOnlyList<Notification> Notifications => CentralBankContext.Notifications.ToList();
        private CentralBankContext CentralBankContext { get; }

        public Bank FindBank(Guid bankId)
        {
            return CentralBankContext.Banks.Find(bankId);
        }

        public BaseBill FindBill(Guid billId)
        {
            return CentralBankContext.Bills.Find(billId);
        }

        public Client FindClient(Guid clientId)
        {
            return CentralBankContext.Clients.Find(clientId);
        }

        public Transaction FindTransaction(Guid transactionId)
        {
            return CentralBankContext.Transactions.Find(transactionId);
        }

        public Notification FindNotification(Guid notificationId)
        {
            return CentralBankContext.Notifications.Find(notificationId);
        }

        public Guid MakeTransaction(Guid billFromId, Guid billToId, decimal money)
        {
            if (CentralBankContext.Bills.Find(billFromId) is null)
            {
                throw new BanksException("BillFrom has not been registered");
            }

            if (CentralBankContext.Bills.Find(billToId) is null)
            {
                throw new BanksException("BillTo has not been registered");
            }

            BaseBill billFrom = CentralBankContext.Bills.Find(billFromId);
            BaseBill billTo = CentralBankContext.Bills.Find(billToId);

            if (!billFrom.Reliable && money > billFrom.UnreliableLimit)
            {
                throw new BanksException(
                    $"BillFrom has Unreliable limit\nLimit: {billFrom.UnreliableLimit}\nTried: {money}");
            }

            if (!billTo.Reliable && money > billTo.UnreliableLimit)
            {
                throw new BanksException(
                    "BillTo has Unreliable limit\nLimit: {billFrom.UnreliableLimit}\nTried: {money}");
            }

            billFrom.Money -= money;
            billTo.Money += money;

            CentralBankContext.Bills.Update(billFrom);
            CentralBankContext.Bills.Update(billTo);

            var transaction = new Transaction(billFromId, billToId, money);
            CentralBankContext.Transactions.Add(transaction);

            CentralBankContext.SaveChanges();
            return transaction.Id;
        }

        public Guid MakeBankTransaction(Guid bankId, Guid billToId, decimal money)
        {
            if (CentralBankContext.Banks.Find(bankId) is null)
            {
                throw new BanksException("Bank has not been registered");
            }

            if (CentralBankContext.Bills.Find(billToId) is null)
            {
                throw new BanksException("BillTo has not been registered");
            }

            BaseBill baseBillTo = CentralBankContext.Bills.Find(billToId);

            baseBillTo.Money += money;

            CentralBankContext.Bills.Update(baseBillTo);

            var transaction = new Transaction(bankId, billToId, money);
            CentralBankContext.Transactions.Add(transaction);

            CentralBankContext.SaveChanges();
            return transaction.Id;
        }

        public void CancelTransaction(Guid id)
        {
            if (CentralBankContext.Transactions.Find(id) is null)
            {
                throw new BanksException("There is no such transaction");
            }

            Transaction transaction = CentralBankContext.Transactions.Find(id);
            if (CentralBankContext.Banks.Find(transaction.From) is not null)
            {
                throw new BanksException("You can't cancel bank transaction");
            }

            if (transaction.Valid == false)
            {
                throw new BanksException("Transaction already canceled");
            }

            Guid backTransaction = MakeTransaction(transaction.To, transaction.From, transaction.Money);
            transaction.Valid = false;
            CentralBankContext.Transactions.Update(transaction);

            CentralBankContext.SaveChanges();
        }

        public Guid RegisterClient(ClientInfo clientInfo)
        {
            if (clientInfo is null)
            {
                throw new BanksException("Client's info is null");
            }

            var client = new Client(clientInfo);
            CentralBankContext.Clients.Add(client);

            CentralBankContext.SaveChanges();
            return client.Id;
        }

        public void AddClientInfo(Guid id, string address, int passport)
        {
            Client client = CentralBankContext.Clients.Find(id);
            client.Address = address ?? throw new BanksException("Address is null");
            client.Passport = passport;
            client.Reliable = true;

            foreach (BaseBill bill in CentralBankContext.Bills)
            {
                if (bill.ClientId == id)
                {
                    bill.Reliable = true;
                }
            }
        }

        public Guid RegisterBank(BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }

            var bank = new Bank(bankInfo);

            CentralBankContext.Banks.Add(bank);

            CentralBankContext.SaveChanges();
            return bank.Id;
        }

        public Guid OpenBill(DepositBillInfo billInfo)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (CentralBankContext.Banks.Find(billInfo.BankId) is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (CentralBankContext.Clients.Find(billInfo.ClientId) is null)
            {
                throw new BanksException("This client has not been registered");
            }

            Bank bank = CentralBankContext.Banks.Find(billInfo.BankId);
            billInfo.Percent = bank.GetDepositPercent(billInfo.Money);
            billInfo.EndDate = DateTime.Now.AddYears(bank.BillDurationYears);
            billInfo.UnreliableLimit = bank.UnreliableLimit;

            var bill = new DepositBill(billInfo)
            {
                OpenDate = DateTime.Now,
                Reliable = CentralBankContext.Clients.Find(billInfo.ClientId).Reliable,
            };

            CentralBankContext.Bills.Add(bill);

            bank.AddClient(CentralBankContext.Clients.Find(billInfo.ClientId));
            CentralBankContext.Banks.Update(bank);

            CentralBankContext.SaveChanges();
            return bill.Id;
        }

        public Guid OpenBill(DebitBillInfo billInfo)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (CentralBankContext.Banks.Find(billInfo.BankId) is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (CentralBankContext.Clients.Find(billInfo.ClientId) is null)
            {
                throw new BanksException("This client has not been registered");
            }

            Bank bank = CentralBankContext.Banks.Find(billInfo.BankId);
            billInfo.Percent = bank.GetDepositPercent(billInfo.Money);
            billInfo.EndDate = DateTime.Now.AddYears(bank.BillDurationYears);
            billInfo.UnreliableLimit = bank.UnreliableLimit;

            var bill = new DebitBill(billInfo)
            {
                OpenDate = DateTime.Now,
                Reliable = CentralBankContext.Clients.Find(billInfo.ClientId).Reliable,
            };

            CentralBankContext.Bills.Add(bill);

            bank.AddClient(CentralBankContext.Clients.Find(billInfo.ClientId));
            CentralBankContext.Banks.Update(bank);

            CentralBankContext.SaveChanges();
            return bill.Id;
        }

        public Guid OpenBill(CreditBillInfo billInfo)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (CentralBankContext.Banks.Find(billInfo.BankId) is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (CentralBankContext.Clients.Find(billInfo.ClientId) is null)
            {
                throw new BanksException("This client has not been registered");
            }

            Bank bank = CentralBankContext.Banks.Find(billInfo.BankId);
            billInfo.Commission = CentralBankContext.Banks.Find(billInfo.BankId).CreditCommission;
            billInfo.Limit = CentralBankContext.Banks.Find(billInfo.BankId).Limit;
            billInfo.EndDate = DateTime.Now.AddYears(bank.BillDurationYears);
            billInfo.UnreliableLimit = bank.UnreliableLimit;

            var bill = new CreditBill(billInfo)
            {
                OpenDate = DateTime.Now,
                Reliable = CentralBankContext.Clients.Find(billInfo.ClientId).Reliable,
            };

            CentralBankContext.Bills.Add(bill);

            bank.AddClient(CentralBankContext.Clients.Find(billInfo.ClientId));
            CentralBankContext.Banks.Update(bank);

            CentralBankContext.SaveChanges();
            return bill.Id;
        }

        public void RewindTime(DateTime targetDate)
        {
            foreach (BaseBill bill in CentralBankContext.Bills)
            {
                for (DateTime i = DateNow; i < targetDate; i = i.AddDays(1))
                {
                    ServiceBill(bill, i);
                }
            }

            DateNow = targetDate;
            CentralBankContext.SaveChanges();
        }

        public void EnableNotification(Guid clientId)
        {
            Client client = CentralBankContext.Clients.Find(clientId);
            client.EnableNotification = true;
            CentralBankContext.Clients.Update(client);

            CentralBankContext.SaveChanges();
        }

        public void ForbidNotification(Guid clientId)
        {
            Client client = CentralBankContext.Clients.Find(clientId);
            client.EnableNotification = false;
            CentralBankContext.Clients.Update(client);

            CentralBankContext.SaveChanges();
        }

        public void ChangeBankInfo(Guid bankId, BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }

            Bank bank = CentralBankContext.Banks.Find(bankId);
            bank.ChangeInfo(bankInfo);

            CentralBankContext.Banks.Update(bank);

            foreach (Client client in bank.Clients)
            {
                if (!client.EnableNotification) continue;

                CentralBankContext.Notifications.Add(new Notification(
                    client.Id,
                    $"Conditions of your bank: {bank.Id} has changed, please check conditions",
                    DateNow));
            }

            CentralBankContext.SaveChanges();
        }

        private void ServiceBill(BaseBill bill, DateTime dateNow)
        {
            if (bill is null)
            {
                throw new BanksException("Bill is null");
            }

            if (bill.EndDate < dateNow)
            {
                throw new BanksException("Your bill is closed");
            }

            if (bill.Money > 0)
            {
                bill.DailyProfits += (bill.DailyProfits + bill.Money) * bill.Percent / (365 * 100);
                CentralBankContext.Bills.Update(bill);
            }

            if (dateNow.Day != 1)
                return;

            MakeBankTransaction(bill.BankId, bill.Id, bill.DailyProfits);
            bill.DailyProfits = 0;
            CentralBankContext.Bills.Update(bill);

            if (bill.Money < 0)
            {
                MakeBankTransaction(bill.BankId, bill.Id, -bill.Commission);
            }

            CentralBankContext.SaveChanges();
        }
    }
}