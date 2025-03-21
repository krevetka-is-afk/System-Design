using System;
using System.IO;
using System.Threading.Tasks;
using FinanceManager.ConsoleUI.Menu;
using FinanceManager.ConsoleUI.Services;
using FinanceManager.Core.Factories;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Services;
using FinanceManager.Infrastructure.Data;
using FinanceManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinanceManager.ConsoleUI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            try
            {
                await InitializeDatabaseAsync(serviceProvider);
                await RunApplicationAsync(serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Database
            var dbPath = Path.Combine(AppContext.BaseDirectory, "finance.db");
            services.AddDbContext<FinanceDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Repositories
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();

            // Services
            services.AddScoped<IFinanceFactory, FinanceFactory>();
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddScoped<IConsoleService, ConsoleService>();
            services.AddScoped<IDataExporter, DataExporter>();

            // Logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFile("logs/finance.log");
            });

            return services;
        }

        private static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
            await context.Database.MigrateAsync();
        }

        private static async Task RunApplicationAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var consoleService = scope.ServiceProvider.GetRequiredService<IConsoleService>();
            var menuManager = new MenuManager();

            // Account operations
            menuManager.AddMenuItem("1", new MenuItem("Create Account", "Create a new bank account", consoleService.CreateAccountAsync));
            menuManager.AddMenuItem("2", new MenuItem("List Accounts", "View all bank accounts", consoleService.ListAccountsAsync));
            menuManager.AddMenuItem("3", new MenuItem("Delete Account", "Delete a bank account", consoleService.DeleteAccountAsync));

            // Category operations
            menuManager.AddMenuItem("4", new MenuItem("Create Category", "Create a new category", consoleService.CreateCategoryAsync));
            menuManager.AddMenuItem("5", new MenuItem("List Categories", "View all categories", consoleService.ListCategoriesAsync));
            menuManager.AddMenuItem("6", new MenuItem("Delete Category", "Delete a category", consoleService.DeleteCategoryAsync));

            // Operation operations
            menuManager.AddMenuItem("7", new MenuItem("Add Operation", "Add a new operation", consoleService.CreateOperationAsync));
            menuManager.AddMenuItem("8", new MenuItem("List Operations", "View all operations", consoleService.ListOperationsAsync));
            menuManager.AddMenuItem("9", new MenuItem("Delete Operation", "Delete an operation", consoleService.DeleteOperationAsync));

            // Analytics
            menuManager.AddMenuItem("A", new MenuItem("View Balance", "View total balance", consoleService.ViewBalanceAsync));
            menuManager.AddMenuItem("B", new MenuItem("View Statistics", "View financial statistics", consoleService.ViewStatisticsAsync));

            // Export
            menuManager.AddMenuItem("E", new MenuItem("Export Data", "Export data to file", consoleService.ExportDataAsync));

            await menuManager.RunAsync();
        }
    }
}
