using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManager.Core.Factories;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Models;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Core.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IBankAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IFinanceFactory _factory;
        private readonly ILogger<FinanceService> _logger;

        public FinanceService(
            IBankAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            IOperationRepository operationRepository,
            IFinanceFactory factory,
            ILogger<FinanceService> logger)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _operationRepository = operationRepository;
            _factory = factory;
            _logger = logger;
        }

        // Account operations
        public async Task<BankAccount> CreateAccountAsync(string name, string currency, decimal initialBalance = 0)
        {
            try
            {
                var account = _factory.CreateBankAccount(name, currency, initialBalance);
                await _accountRepository.AddAsync(account);
                _logger.LogInformation("Created bank account: {AccountName}", name);
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create bank account: {AccountName}", name);
                throw;
            }
        }

        public async Task<BankAccount> GetAccountAsync(Guid id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task UpdateAccountAsync(BankAccount account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            await _accountRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _accountRepository.GetTotalBalanceAsync();
        }

        // Category operations
        public async Task<Category> CreateCategoryAsync(string name, CategoryType type, string description = "")
        {
            try
            {
                var category = _factory.CreateCategory(name, type, description);
                await _categoryRepository.AddAsync(category);
                _logger.LogInformation("Created category: {CategoryName} ({CategoryType})", name, type);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category: {CategoryName}", name);
                throw;
            }
        }

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type)
        {
            return await _categoryRepository.GetByTypeAsync(type);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        // Financial operations
        public async Task<Operation> CreateOperationAsync(Guid accountId, Guid categoryId, decimal amount, string description = "")
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    throw new ArgumentException("Category not found", nameof(categoryId));

                // Если категория - расход, делаем сумму отрицательной
                if (category.Type == CategoryType.Expense)
                    amount = -Math.Abs(amount);
                else
                    amount = Math.Abs(amount);

                var operation = _factory.CreateOperation(accountId, categoryId, amount, description);
                await _operationRepository.AddAsync(operation);
                await _accountRepository.UpdateBalanceAsync(accountId, amount);
                _logger.LogInformation("Created operation: {Amount:C} for account {AccountId}", amount, accountId);
                return operation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create operation for account {AccountId}", accountId);
                throw;
            }
        }

        public async Task<Operation> GetOperationAsync(Guid id)
        {
            return await _operationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Operation>> GetAllOperationsAsync()
        {
            return await _operationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Operation>> GetOperationsByAccountAsync(Guid accountId)
        {
            return await _operationRepository.GetByAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<Operation>> GetOperationsByCategoryAsync(Guid categoryId)
        {
            return await _operationRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Operation>> GetOperationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _operationRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task UpdateOperationAsync(Operation operation)
        {
            await _operationRepository.UpdateAsync(operation);
        }

        public async Task DeleteOperationAsync(Guid id)
        {
            await _operationRepository.DeleteAsync(id);
        }

        // Analytics
        public async Task<decimal> GetBalanceByAccountAsync(Guid accountId)
        {
            return await _operationRepository.GetTotalByAccountIdAsync(accountId);
        }

        public async Task<decimal> GetTotalByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var operations = await _operationRepository.GetByDateRangeAsync(startDate, endDate);
            return operations.Sum(o => o.Amount);
        }

        public async Task<IDictionary<Category, decimal>> GetExpensesByCategoryAsync(DateTime startDate, DateTime endDate)
        {
            var expenseCategories = await _categoryRepository.GetByTypeAsync(CategoryType.Expense);
            var result = new Dictionary<Category, decimal>();

            foreach (var category in expenseCategories)
            {
                var total = await _operationRepository.GetTotalByCategoryIdAsync(category.Id);
                if (total != 0)
                {
                    result[category] = Math.Abs(total);
                }
            }

            return result;
        }

        public async Task<IDictionary<Category, decimal>> GetIncomeByCategoryAsync(DateTime startDate, DateTime endDate)
        {
            var incomeCategories = await _categoryRepository.GetByTypeAsync(CategoryType.Income);
            var result = new Dictionary<Category, decimal>();

            foreach (var category in incomeCategories)
            {
                var total = await _operationRepository.GetTotalByCategoryIdAsync(category.Id);
                if (total != 0)
                {
                    result[category] = total;
                }
            }

            return result;
        }
    }
} 