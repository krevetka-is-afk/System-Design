using System;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Interfaces
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        Task<decimal> GetTotalBalanceAsync();
        Task UpdateBalanceAsync(Guid accountId, decimal amount);
    }
} 