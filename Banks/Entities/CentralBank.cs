using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Contexts;
using Banks.Entities.Bills;
using Banks.Models;
using Banks.Models.Builders;
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
            BaseBill billFrom = CentralBankContext.Bills.Find(billFromId);
            BaseBill billTo = CentralBankContext.Bills.Find(billToId);

            var transaction = new Transaction(billFrom, billTo, money);
            CentralBankContext.Transactions.Add(transaction);

            CentralBankContext.SaveChanges();
            return transaction.Id;
        }

        public void CancelTransaction(Guid id)
        {
            Transaction transaction = CentralBankContext.Transactions.Find(id);

            Checks.CancelTransactionChecks(transaction, CentralBankContext.Banks);

            MakeTransaction(transaction.To, transaction.From, transaction.Money);
            transaction.Valid = false;
            CentralBankContext.Transactions.Update(transaction);

            CentralBankContext.SaveChanges();
        }

        public Guid RegisterClient(ClientBuilder clientBuilder)
        {
            Checks.RegisterClientChecks(clientBuilder);

            Client client = clientBuilder.Build();
            CentralBankContext.Clients.Add(client);

            CentralBankContext.SaveChanges();
            return client.Id;
        }

        public void AddClientInfo(Guid id, string address, int passport)
        {
            Client client = CentralBankContext.Clients.Find(id);

            Checks.AddClientInfoChecks(client, address);

            client.AddClientInfo(address, passport);

            foreach (BaseBill bill in CentralBankContext.Bills)
            {
                if (bill.Client.Id == id)
                {
                    bill.Reliable = true;
                }
            }

            CentralBankContext.SaveChanges();
        }

        public Guid RegisterBank(BankBuilder bankBuilder)
        {
            Checks.RegisterBankChecks(bankBuilder);

            Bank bank = bankBuilder.Build();

            CentralBankContext.Banks.Add(bank);

            CentralBankContext.SaveChanges();
            return bank.Id;
        }

        public Guid OpenBill(BaseBillBuilder billBuilder)
        {
            Checks.OpenBillChecks(billBuilder, CentralBankContext);

            Bank bank = CentralBankContext.Banks.Find(billBuilder.Bank.Id);
            Client client = CentralBankContext.Clients.Find(billBuilder.Client.Id);

            billBuilder.AddBankInfo(
                bank.GetDepositPercent(billBuilder.Money),
                bank.Limit,
                bank.CreditCommission,
                bank.UnreliableLimit,
                DateNow,
                DateNow.AddYears(bank.BillDurationYears),
                client.Reliable);

            BaseBill bill = billBuilder.CreateBill();
            CentralBankContext.Bills.Add(bill);

            bank.AddClient(client);
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

            Checks.NotificationChecks(client);

            client.EnableNotification = true;
            CentralBankContext.Clients.Update(client);

            CentralBankContext.SaveChanges();
        }

        public void ForbidNotification(Guid clientId)
        {
            Client client = CentralBankContext.Clients.Find(clientId);

            Checks.NotificationChecks(client);

            client.EnableNotification = false;
            CentralBankContext.Clients.Update(client);

            CentralBankContext.SaveChanges();
        }

        public void ChangeBankInfo(Guid bankId, BankBuilder bankBuilder)
        {
            Bank bank = CentralBankContext.Banks.Find(bankId);

            Checks.ChangeBankInfoChecks(bank, bankBuilder);

            bank.ChangeInfo(bankBuilder);
            CentralBankContext.Banks.Update(bank);

            foreach (Client client in bank.Clients.Where(client => client.EnableNotification))
            {
                CentralBankContext.Notifications.Add(
                    new Notification(
                        client.Id,
                        $"Conditions of your bank: {bank.Id} has changed, please check conditions",
                        DateNow));
            }

            CentralBankContext.SaveChanges();
        }

        private void ServiceBill(BaseBill bill, DateTime dateNow)
        {
            Checks.ServiceBillChecks(bill, dateNow);

            bill.ServiceDailyProfits();
            Transaction transactionPercent = bill.ServicePercent(dateNow);
            Transaction transactionCommission = bill.ServiceCommission(dateNow);

            if (transactionPercent is not null)
            {
                CentralBankContext.Transactions.Add(transactionPercent);
            }

            if (transactionCommission is not null)
            {
                CentralBankContext.Transactions.Add(transactionCommission);
            }

            CentralBankContext.SaveChanges();
        }
    }
}