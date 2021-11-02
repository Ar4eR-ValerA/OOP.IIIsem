using System;
using System.Collections.Generic;
using Banks.Contexts;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly List<Guid> _banksIds;
        private readonly List<Guid> _transactionsIds;
        private readonly List<Guid> _clientsIds;
        private readonly List<Guid> _billsIds;

        public CentralBank(CentralBankContext centralBankContext)
        {
            CentralBankContext = centralBankContext;
            _banksIds = new List<Guid>();
            _transactionsIds = new List<Guid>();
            _clientsIds = new List<Guid>();
            _billsIds = new List<Guid>();
        }

        public IReadOnlyList<Guid> BanksIds => _banksIds;
        public IReadOnlyList<Guid> TransactionsIds => _transactionsIds;
        public IReadOnlyList<Guid> ClientsIds => _clientsIds;
        public IReadOnlyList<Guid> BillsIds => _billsIds;
        public CentralBankContext CentralBankContext { get; }

        public Guid MakeTransaction(Guid billFromId, Guid billToId, int amount)
        {
            if (!_billsIds.Contains(billFromId))
            {
                throw new BanksException("BillFrom has not been registered");
            }

            if (!_billsIds.Contains(billToId))
            {
                throw new BanksException("BillTo has not been registered");
            }

            Bill billFrom = CentralBankContext.Bills.Find(billFromId);
            Bill billTo = CentralBankContext.Bills.Find(billToId);

            billFrom.Money -= amount;
            billTo.Money += amount;

            CentralBankContext.Bills.Update(billFrom);
            CentralBankContext.Bills.Update(billTo);

            var transaction = new Transaction(billFromId, billToId, amount);
            _transactionsIds.Add(transaction.Id);
            CentralBankContext.Transactions.Add(transaction);

            CentralBankContext.SaveChanges();
            return transaction.Id;
        }

        public void CancelTransaction(Guid id)
        {
            Transaction transaction = CentralBankContext.Transactions.Find(id);
            MakeTransaction(transaction.To, transaction.From, transaction.Amount);

            CentralBankContext.SaveChanges();
        }

        // TODO: Сделать ClientInfo, BankInfo
        public Guid RegisterClient(string name, string surname)
        {
            var client = new Client(name, surname);
            _clientsIds.Add(client.Id);
            CentralBankContext.Clients.Add(client);

            CentralBankContext.SaveChanges();
            return client.Id;
        }

        public Guid RegisterBank(string name)
        {
            var bank = new Bank(name);
            _banksIds.Add(bank.Id);
            CentralBankContext.Banks.Add(bank);

            CentralBankContext.SaveChanges();
            return bank.Id;
        }

        public Guid OpenBill(Guid bankId, Guid clientId, decimal money)
        {
            if (!_banksIds.Contains(bankId))
            {
                throw new BanksException("This bank has not been registered");
            }

            if (!_clientsIds.Contains(clientId))
            {
                throw new BanksException("This client has not been registered");
            }

            var bill = new Bill(bankId, clientId, money);
            _billsIds.Add(bill.Id);
            CentralBankContext.Bills.Add(bill);

            CentralBankContext.SaveChanges();
            return bill.Id;
        }
    }
}