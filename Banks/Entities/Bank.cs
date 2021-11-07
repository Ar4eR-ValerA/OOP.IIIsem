using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.Models;
using Banks.Models.Infos;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        private List<DepositMoneyGap> _depositMoneyGaps;
        private List<Client> _clients;

        public Bank(BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }

            Name = bankInfo.Name;
            DebitPercent = bankInfo.DebitPercent;
            _depositMoneyGaps = bankInfo.DepositMoneyGaps;
            CreditCommission = bankInfo.CreditCommission;
            Limit = bankInfo.Limit;
            _clients = new List<Client>();
            Id = Guid.NewGuid();
            BillDurationYears = bankInfo.BillDurationYears;
        }

        public Bank()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal DebitPercent { get; private set; }

        public decimal CreditCommission { get; private set; }
        public decimal Limit { get; private set; }
        public int BillDurationYears { get; private set; }
        public decimal UnreliableLimit { get; private set; }

        [NotMapped]
        public IReadOnlyList<DepositMoneyGap> DepositMoneyGaps => _depositMoneyGaps;

        [NotMapped]
        public IReadOnlyList<Client> Clients => _clients;

        public decimal GetDepositPercent(decimal money)
        {
            foreach (DepositMoneyGap depositMoneyGap in DepositMoneyGaps)
            {
                if (depositMoneyGap.InMoneyGap(money))
                {
                    return depositMoneyGap.Percent;
                }
            }

            throw new BanksException("There is no suitable money gap for your money");
        }

        internal void ChangeInfo(BankInfo bankInfo)
        {
            if (bankInfo is null)
            {
                throw new BanksException("Bank's info is null");
            }

            Name = bankInfo.Name;
            DebitPercent = bankInfo.DebitPercent;
            _depositMoneyGaps = bankInfo.DepositMoneyGaps;
            CreditCommission = bankInfo.CreditCommission;
            Limit = bankInfo.Limit;
            BillDurationYears = bankInfo.BillDurationYears;
        }

        internal void AddClient(Client client)
        {
            if (client is null)
            {
                throw new BanksException("Client is null");
            }

            if (!Clients.Contains(client))
                _clients.Add(client);
        }
    }
}