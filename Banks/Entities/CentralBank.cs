using System;
using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly List<Guid> _banksIds;
        private readonly List<Guid> _transactionsIds;
        private readonly List<Guid> _clientsIds;
        private readonly List<Guid> _billsIds;

        public CentralBank(ICentralBankRepository centralBankRepository)
        {
            Repository = centralBankRepository;
            _banksIds = new List<Guid>();
            _transactionsIds = new List<Guid>();
            _clientsIds = new List<Guid>();
            _billsIds = new List<Guid>();
        }

        public IReadOnlyList<Guid> BanksIds => _banksIds;
        public IReadOnlyList<Guid> TransactionsIds => _transactionsIds;
        public IReadOnlyList<Guid> ClientsIds => _clientsIds;
        public IReadOnlyList<Guid> BillsIds => _billsIds;
        public ICentralBankRepository Repository { get; }

        // Guid MakeTransaction(Guid from, Guid to, int amount)
        // void CancelTransaction(Guid guid)
        // Guid RegisterClient(Client client)
        // Guid RegisterBank(Bank bank)
        public Guid OpenBill(Guid bankId, Guid clientId, decimal money)
        {
            // TODO: Проверить, что айдишники дейстивтельные
            var bill = new Bill(bankId, clientId, money);
            _billsIds.Add(bill.Id);
            Repository.AddBill(bill);

            return bill.Id;
        }
    }
}