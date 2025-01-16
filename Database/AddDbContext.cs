using Microsoft.EntityFrameworkCore;
using PersonalExpenseTracker.Components.Pages;
using System.IO;

namespace PersonalExpenseTracker.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Debt> Debts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "expenses.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}