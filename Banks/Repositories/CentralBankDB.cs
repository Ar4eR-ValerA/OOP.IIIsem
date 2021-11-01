using System.Collections.Generic;
using System.Linq;
using Banks.Contexts;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Repositories
{
    public class CentralBankDb : ICentralBankRepository
    {
        public CentralBankDb(CentralBankContext centralBankContext)
        {
            CentralBankContext = centralBankContext;
        }

        public IReadOnlyList<Client> Clients => CentralBankContext.Clients.ToList();
        public IReadOnlyList<Bank> Banks => CentralBankContext.Banks.ToList();
        public IReadOnlyList<Transaction> Transactions => CentralBankContext.Transactions.ToList();
        public IReadOnlyList<Bill> Bills => CentralBankContext.Bills.ToList();

        private CentralBankContext CentralBankContext { get; }

        public void AddClient(Client client)
        {
            CentralBankContext.Clients.Add(client);
        }

        public void AddBank(Bank bank)
        {
            CentralBankContext.Banks.Add(bank);
        }

        public void AddTransaction(Transaction transaction)
        {
            CentralBankContext.Transactions.Add(transaction);
        }

        public void AddBill(Bill bill)
        {
            CentralBankContext.Bills.Add(bill);
        }
    }
}