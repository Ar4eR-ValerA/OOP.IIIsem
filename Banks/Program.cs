using Banks.Contexts;
using Banks.Entities;
using Banks.Repositories;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var db = new CentralBankContext("file.db");
            var bank = new Bank("ads");
            var client = new Client("ads", "fd");
            var centralBank = new CentralBank(new CentralBankDb(db));
            centralBank.Repository.AddBank(bank);
            centralBank.Repository.AddClient(client);
            centralBank.OpenBill(bank.Id, client.Id, 10);
            db.SaveChanges();
        }
    }
}