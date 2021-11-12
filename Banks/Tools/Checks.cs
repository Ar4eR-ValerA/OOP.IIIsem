using System;
using System.Linq;
using Banks.Entities;
using Banks.Entities.Bills;
using Banks.Models.Infos;
using Microsoft.EntityFrameworkCore;

namespace Banks.Tools
{
    public static class Checks
    {
        public static void MakeTransactionChecks(BaseBill billFrom, BaseBill billTo, decimal money)
        {
            if (billFrom is null)
            {
                throw new BanksException("BillFrom has not been registered");
            }

            if (billTo is null)
            {
                throw new BanksException("BillTo has not been registered");
            }

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
        }

        public static void MakeBankTransactionChecks(Bank bank, BaseBill billTo)
        {
            if (bank is null)
            {
                throw new BanksException("Bank has not been registered");
            }

            if (billTo is null)
            {
                throw new BanksException("BillTo has not been registered");
            }
        }

        public static void CancelTransactionChecks(Transaction transaction, DbSet<Bank> banks)
        {
            if (transaction is null)
            {
                throw new BanksException("There is no such transaction");
            }

            if (banks.Find(transaction.From) is not null)
            {
                throw new BanksException("You can't cancel bank transaction");
            }

            if (transaction.Valid == false)
            {
                throw new BanksException("Transaction already canceled");
            }
        }

        public static void RegisterClientChecks(ClientInfo clientInfo)
        {
            if (clientInfo is null)
            {
                throw new BanksException("Client's info is null");
            }
        }

        public static void AddClientInfoChecks(Client client, string address)
        {
            if (address is null)
            {
                throw new BanksException("Address is null");
            }

            if (client is null)
            {
                throw new BanksException("This client has not been registered");
            }
        }

        public static void RegisterBankChecks(BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }
        }

        public static void OpenBillChecks(DepositBillInfo billInfo, Bank bank, Client client)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (bank is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (client is null)
            {
                throw new BanksException("This client has not been registered");
            }
        }

        public static void OpenBillChecks(CreditBillInfo billInfo, Bank bank, Client client)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (bank is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (client is null)
            {
                throw new BanksException("This client has not been registered");
            }
        }

        public static void OpenBillChecks(DebitBillInfo billInfo, Bank bank, Client client)
        {
            if (billInfo is null)
            {
                throw new BanksException("Bill's info is null");
            }

            if (bank is null)
            {
                throw new BanksException("This bank has not been registered");
            }

            if (client is null)
            {
                throw new BanksException("This client has not been registered");
            }
        }

        public static void ChangeBankInfoChecks(Bank bank, BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }

            if (bank is null)
            {
                throw new BanksException("This bank has not been registered");
            }
        }

        public static void ServiceBillChecks(BaseBill bill, DateTime dateNow)
        {
            if (bill is null)
            {
                throw new BanksException("Bill is null");
            }

            if (bill.EndDate < dateNow)
            {
                throw new BanksException("Your bill is closed");
            }
        }
    }
}