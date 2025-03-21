using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanceManager.Infrastructure.Data
{
    public class FinanceDbContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
    {
        public FinanceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            var dbPath = Path.Combine(AppContext.BaseDirectory, "finance.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new FinanceDbContext(optionsBuilder.Options);
        }
    }
} 