using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<IEnumerable<Operation>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Operation>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Operation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalByAccountIdAsync(Guid accountId);
        Task<decimal> GetTotalByCategoryIdAsync(Guid categoryId);
    }
} 