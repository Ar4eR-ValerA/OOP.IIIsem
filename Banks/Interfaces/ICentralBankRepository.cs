using System.Collections.Generic;
using Banks.Entities;

namespace Banks.Interfaces
{
    public interface ICentralBankRepository
    {
        IReadOnlyList<Client> Clients { get; }
        IReadOnlyList<Bank> Banks { get; }
        IReadOnlyList<Transaction> Transactions { get; }
        IReadOnlyList<Bill> Bills { get; }

        void AddClient(Client client);
        void AddBank(Bank bank);
        void AddTransaction(Transaction transaction);
        void AddBill(Bill bill);
    }
}