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
    public class OperationRepository : IOperationRepository
    {
        private readonly FinanceDbContext _context;
        private readonly ILogger<OperationRepository> _logger;

        public OperationRepository(FinanceDbContext context, ILogger<OperationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Operation> GetByIdAsync(Guid id)
        {
            return await _context.Operations.FindAsync(id);
        }

        public async Task<IEnumerable<Operation>> GetAllAsync()
        {
            return await _context.Operations.ToListAsync();
        }

        public async Task<Operation> AddAsync(Operation entity)
        {
            await _context.Operations.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added operation: {Amount:C} for account {AccountId}", entity.Amount, entity.AccountId);
            return entity;
        }

        public async Task UpdateAsync(Operation entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated operation: {OperationId}", entity.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var operation = await GetByIdAsync(id);
            if (operation != null)
            {
                _context.Operations.Remove(operation);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted operation: {OperationId}", id);
            }
        }

        public async Task<IEnumerable<Operation>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Operations
                .Where(o => o.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Operations
                .Where(o => o.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Operations
                .Where(o => o.Date >= startDate && o.Date <= endDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByAccountIdAsync(Guid accountId)
        {
            var operations = await _context.Operations
                .Where(o => o.AccountId == accountId)
                .ToListAsync();
            return operations.Sum(o => o.Amount);
        }

        public async Task<decimal> GetTotalByCategoryIdAsync(Guid categoryId)
        {
            var operations = await _context.Operations
                .Where(o => o.CategoryId == categoryId)
                .ToListAsync();
            return operations.Sum(o => o.Amount);
        }
    }
} 