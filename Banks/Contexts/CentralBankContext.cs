using Banks.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banks.Contexts
{
    public sealed class CentralBankContext : DbContext
    {
        public CentralBankContext(string fileName)
        {
            FileName = fileName;
            Database.EnsureCreated();
        }

        internal DbSet<Client> Clients { get; set; }
        internal DbSet<Bank> Banks { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }
        internal DbSet<Bill> Bills { get; set; }

        private string FileName { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={FileName}");
        }
    }
}