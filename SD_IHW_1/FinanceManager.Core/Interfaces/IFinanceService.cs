using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Interfaces
{
    public interface IFinanceService
    {
        // Account operations
        Task<BankAccount> CreateAccountAsync(string name, string currency, decimal initialBalance = 0);
        Task<BankAccount> GetAccountAsync(Guid id);
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();
        Task UpdateAccountAsync(BankAccount account);
        Task DeleteAccountAsync(Guid id);
        Task<decimal> GetTotalBalanceAsync();

        // Category operations
        Task<Category> CreateCategoryAsync(string name, CategoryType type, string description = "");
        Task<Category> GetCategoryAsync(Guid id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid id);

        // Financial operations
        Task<Operation> CreateOperationAsync(Guid accountId, Guid categoryId, decimal amount, string description = "");
        Task<Operation> GetOperationAsync(Guid id);
        Task<IEnumerable<Operation>> GetAllOperationsAsync();
        Task<IEnumerable<Operation>> GetOperationsByAccountAsync(Guid accountId);
        Task<IEnumerable<Operation>> GetOperationsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Operation>> GetOperationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task UpdateOperationAsync(Operation operation);
        Task DeleteOperationAsync(Guid id);

        // Analytics
        Task<decimal> GetBalanceByAccountAsync(Guid accountId);
        Task<decimal> GetTotalByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IDictionary<Category, decimal>> GetExpensesByCategoryAsync(DateTime startDate, DateTime endDate);
        Task<IDictionary<Category, decimal>> GetIncomeByCategoryAsync(DateTime startDate, DateTime endDate);
    }
} 