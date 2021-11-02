using System;
using Banks.Contexts;
using Banks.Entities;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var centralBank = new CentralBank(new CentralBankContext("file.db"));

            Guid bankId = centralBank.RegisterBank("Valera's BANK");
            Guid clientId = centralBank.RegisterClient("Valera", "Shevchenko");

            Guid bill1Id = centralBank.OpenBill(bankId, clientId, 100);
            Guid bill2Id = centralBank.OpenBill(bankId, clientId, 200);

            centralBank.MakeTransaction(bill1Id, bill2Id, 5);
            Guid transactionId = centralBank.MakeTransaction(bill1Id, bill2Id, 40);

            centralBank.CancelTransaction(transactionId);
        }
    }
}