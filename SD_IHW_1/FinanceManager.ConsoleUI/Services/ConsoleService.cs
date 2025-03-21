using System;
using System.Threading.Tasks;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Models;
using Microsoft.Extensions.Logging;

namespace FinanceManager.ConsoleUI.Services
{
    public interface IConsoleService
    {
        Task CreateAccountAsync();
        Task ListAccountsAsync();
        Task DeleteAccountAsync();
        Task CreateCategoryAsync();
        Task ListCategoriesAsync();
        Task DeleteCategoryAsync();
        Task CreateOperationAsync();
        Task ListOperationsAsync();
        Task DeleteOperationAsync();
        Task ViewBalanceAsync();
        Task ViewStatisticsAsync();
        Task ExportDataAsync();
    }

    public class ConsoleService : IConsoleService
    {
        private readonly IFinanceService _financeService;
        private readonly IDataExporter _dataExporter;
        private readonly ILogger<ConsoleService> _logger;

        public ConsoleService(
            IFinanceService financeService,
            IDataExporter dataExporter,
            ILogger<ConsoleService> logger)
        {
            _financeService = financeService;
            _dataExporter = dataExporter;
            _logger = logger;
        }

        public async Task CreateAccountAsync()
        {
            try
            {
                Console.WriteLine("\nCreate New Bank Account");
                Console.WriteLine("----------------------");

                Console.Write("Enter account name: ");
                var name = Console.ReadLine();

                Console.Write("Enter currency (e.g. USD): ");
                var currency = Console.ReadLine();

                Console.Write("Enter initial balance: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal balance))
                {
                    var account = await _financeService.CreateAccountAsync(name ?? "", currency ?? "", balance);
                    Console.WriteLine($"\nAccount created successfully! ID: {account.Id}");
                }
                else
                {
                    Console.WriteLine("\nInvalid balance amount. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ListAccountsAsync()
        {
            try
            {
                Console.WriteLine("\nBank Accounts");
                Console.WriteLine("-------------");

                var accounts = await _financeService.GetAllAccountsAsync();
                foreach (var account in accounts)
                {
                    Console.WriteLine($"ID: {account.Id}");
                    Console.WriteLine($"Name: {account.Name}");
                    Console.WriteLine($"Balance: {account.Balance:C2}");
                    Console.WriteLine("-------------");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing accounts");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task DeleteAccountAsync()
        {
            try
            {
                Console.WriteLine("\nDelete Bank Account");
                Console.WriteLine("------------------");

                Console.Write("Enter account ID to delete: ");
                if (Guid.TryParse(Console.ReadLine(), out Guid id))
                {
                    await _financeService.DeleteAccountAsync(id);
                    Console.WriteLine("\nAccount deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nInvalid account ID format. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task CreateCategoryAsync()
        {
            try
            {
                Console.WriteLine("\nCreate New Category");
                Console.WriteLine("------------------");

                Console.Write("Enter category name: ");
                var name = Console.ReadLine();

                Console.Write("Enter category description: ");
                var description = Console.ReadLine();

                Console.WriteLine("Select category type:");
                Console.WriteLine("1. Income");
                Console.WriteLine("2. Expense");
                Console.Write("Enter choice (1 or 2): ");

                if (int.TryParse(Console.ReadLine(), out int choice) && (choice == 1 || choice == 2))
                {
                    var type = choice == 1 ? CategoryType.Income : CategoryType.Expense;
                    var category = await _financeService.CreateCategoryAsync(name ?? "", type, description ?? "");
                    Console.WriteLine($"\nCategory created successfully! ID: {category.Id}");
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ListCategoriesAsync()
        {
            try
            {
                Console.WriteLine("\nCategories");
                Console.WriteLine("----------");

                var categories = await _financeService.GetAllCategoriesAsync();
                foreach (var category in categories)
                {
                    Console.WriteLine($"ID: {category.Id}");
                    Console.WriteLine($"Name: {category.Name}");
                    Console.WriteLine($"Description: {category.Description}");
                    Console.WriteLine($"Type: {category.Type}");
                    Console.WriteLine("----------");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing categories");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task DeleteCategoryAsync()
        {
            try
            {
                Console.WriteLine("\nDelete Category");
                Console.WriteLine("---------------");

                Console.Write("Enter category ID to delete: ");
                if (Guid.TryParse(Console.ReadLine(), out Guid id))
                {
                    await _financeService.DeleteCategoryAsync(id);
                    Console.WriteLine("\nCategory deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nInvalid category ID format. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task CreateOperationAsync()
        {
            try
            {
                Console.WriteLine("\nCreate New Operation");
                Console.WriteLine("-------------------");

                Console.Write("Enter account ID: ");
                if (!Guid.TryParse(Console.ReadLine(), out Guid accountId))
                {
                    Console.WriteLine("\nInvalid account ID format. Please try again.");
                    return;
                }

                var account = await _financeService.GetAccountAsync(accountId);
                if (account == null)
                {
                    Console.WriteLine("\nAccount not found. Please try again.");
                    return;
                }
                Console.WriteLine($"Selected account: {account.Name} (Balance: {account.Balance:C2})");

                Console.Write("Enter category ID: ");
                if (!Guid.TryParse(Console.ReadLine(), out Guid categoryId))
                {
                    Console.WriteLine("\nInvalid category ID format. Please try again.");
                    return;
                }

                var category = await _financeService.GetCategoryAsync(categoryId);
                if (category == null)
                {
                    Console.WriteLine("\nCategory not found. Please try again.");
                    return;
                }
                Console.WriteLine($"Selected category: {category.Name} (Type: {category.Type})");

                Console.Write($"Enter amount ({(category.Type == CategoryType.Expense ? "expense" : "income")}): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("\nInvalid amount format. Please try again.");
                    return;
                }

                Console.Write("Enter description: ");
                var description = Console.ReadLine();

                var operation = await _financeService.CreateOperationAsync(accountId, categoryId, amount, description ?? "");
                Console.WriteLine($"\nOperation created successfully! ID: {operation.Id}");
                Console.WriteLine($"New balance: {(await _financeService.GetAccountAsync(accountId))?.Balance:C2}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operation");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ListOperationsAsync()
        {
            try
            {
                Console.WriteLine("\nOperations");
                Console.WriteLine("----------");

                var operations = await _financeService.GetAllOperationsAsync();
                foreach (var operation in operations)
                {
                    Console.WriteLine($"ID: {operation.Id}");
                    Console.WriteLine($"Account ID: {operation.AccountId}");
                    Console.WriteLine($"Category ID: {operation.CategoryId}");
                    Console.WriteLine($"Amount: {operation.Amount:C2}");
                    Console.WriteLine($"Description: {operation.Description}");
                    Console.WriteLine($"Date: {operation.Date}");
                    Console.WriteLine("----------");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing operations");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task DeleteOperationAsync()
        {
            try
            {
                Console.WriteLine("\nDelete Operation");
                Console.WriteLine("----------------");

                Console.Write("Enter operation ID to delete: ");
                if (Guid.TryParse(Console.ReadLine(), out Guid id))
                {
                    await _financeService.DeleteOperationAsync(id);
                    Console.WriteLine("\nOperation deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nInvalid operation ID format. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting operation");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ViewBalanceAsync()
        {
            try
            {
                Console.WriteLine("\nTotal Balance");
                Console.WriteLine("-------------");

                var totalBalance = await _financeService.GetTotalBalanceAsync();
                Console.WriteLine($"Total Balance: {totalBalance:C2}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing balance");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ViewStatisticsAsync()
        {
            try
            {
                Console.WriteLine("\nFinancial Statistics");
                Console.WriteLine("-------------------");

                var accounts = await _financeService.GetAllAccountsAsync();
                foreach (var account in accounts)
                {
                    Console.WriteLine($"\nAccount: {account.Name}");
                    Console.WriteLine($"Balance: {account.Balance:C2}");

                    var operations = await _financeService.GetOperationsByAccountAsync(account.Id);
                    var income = operations.Where(o => o.Amount > 0).Sum(o => o.Amount);
                    var expenses = operations.Where(o => o.Amount < 0).Sum(o => o.Amount);

                    Console.WriteLine($"Total Income: {income:C2}");
                    Console.WriteLine($"Total Expenses: {Math.Abs(expenses):C2}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing statistics");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        public async Task ExportDataAsync()
        {
            try
            {
                Console.WriteLine("\nExport Data");
                Console.WriteLine("-----------");

                Console.WriteLine("Select export format:");
                Console.WriteLine("1. JSON");
                Console.WriteLine("2. CSV");
                Console.WriteLine("3. YAML");
                Console.Write("Enter choice (1-3): ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
                {
                    var format = choice switch
                    {
                        1 => "json",
                        2 => "csv",
                        3 => "yaml",
                        _ => "json"
                    };

                    Console.Write("Enter export directory path: ");
                    var path = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(path))
                    {
                        path = "exports";
                    }

                    await _dataExporter.ExportAsync(path, format);
                    Console.WriteLine($"\nData exported successfully to {path}!");
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting data");
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
    }
} 