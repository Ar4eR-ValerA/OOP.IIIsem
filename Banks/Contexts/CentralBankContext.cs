using Banks.Entities;
using Banks.Entities.Bills;
using Banks.Models;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;

namespace Banks.Contexts
{
    public sealed class CentralBankContext : DbContext
    {
        public CentralBankContext(string fileName)
        {
            FileName = fileName ?? throw new BanksException("File name is null");
            Database.EnsureCreated();
        }

        internal DbSet<Client> Clients { get; set; }
        internal DbSet<Bank> Banks { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }
        internal DbSet<BaseBill> Bills { get; set; }
        internal DbSet<DebitBill> DebitBills { get; set; }
        internal DbSet<DepositBill> DepositBills { get; set; }
        internal DbSet<CreditBill> CreditBills { get; set; }
        internal DbSet<Notification> Notifications { get; set; }

        private string FileName { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase($"Filename={FileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>()
                .HasMany("_depositMoneyGaps");
            modelBuilder.Entity<Bank>().HasMany("_clients");
        }
    }
}