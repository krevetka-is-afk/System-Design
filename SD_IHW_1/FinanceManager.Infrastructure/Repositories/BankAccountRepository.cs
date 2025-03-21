using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Models;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly FinanceDbContext _context;
        private readonly ILogger<BankAccountRepository> _logger;

        public BankAccountRepository(FinanceDbContext context, ILogger<BankAccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BankAccount> GetByIdAsync(Guid id)
        {
            return await _context.BankAccounts.FindAsync(id);
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount> AddAsync(BankAccount entity)
        {
            await _context.BankAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added bank account: {AccountName}", entity.Name);
            return entity;
        }

        public async Task UpdateAsync(BankAccount entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated bank account: {AccountName}", entity.Name);
        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await GetByIdAsync(id);
            if (account != null)
            {
                _context.BankAccounts.Remove(account);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted bank account: {AccountName}", account.Name);
            }
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            var accounts = await _context.BankAccounts.ToListAsync();
            return accounts.Sum(a => a.Balance);
        }

        public async Task UpdateBalanceAsync(Guid accountId, decimal amount)
        {
            var account = await GetByIdAsync(accountId);
            if (account != null)
            {
                account.UpdateBalance(amount);
                await UpdateAsync(account);
                _logger.LogInformation("Updated balance for account {AccountName}: {Amount:C}", account.Name, amount);
            }
        }
    }
} 