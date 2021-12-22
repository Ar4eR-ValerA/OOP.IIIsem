using Microsoft.EntityFrameworkCore;
using Reports.Entities.Employees;
using Reports.Entities.Reports;
using Reports.Entities.Tasks;

namespace Reports.Database
{
    public sealed class ReportsDatabaseContext : DbContext
    {
        public ReportsDatabaseContext(DbContextOptions<ReportsDatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BaseEmployee> BaseEmployees { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<TeamLead> TeamLeads { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().Navigation(m => m.Subordinates).HasField("_subordinates");
            modelBuilder.Entity<TeamLead>().Navigation(t => t.Subordinates).HasField("_subordinates");
            modelBuilder.Entity<Task>().Navigation(t => t.Comments).HasField("_comments");
            modelBuilder.Entity<Report>().Navigation(r => r.Comments).HasField("_comments");
            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}