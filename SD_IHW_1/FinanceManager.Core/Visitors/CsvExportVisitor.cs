using System.IO;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Visitors
{
    public class CsvExportVisitor : IDataVisitor
    {
        private readonly string _outputPath;

        public CsvExportVisitor(string outputPath)
        {
            _outputPath = outputPath;
        }

        public async Task VisitBankAccount(BankAccount account)
        {
            var csvLine = $"{account.Id},{account.Name},{account.Currency},{account.Balance},{account.CreatedAt:yyyy-MM-dd HH:mm:ss}";
            await File.AppendAllTextAsync($"{_outputPath}/accounts.csv", csvLine + "\n");
        }

        public async Task VisitCategory(Category category)
        {
            var csvLine = $"{category.Id},{category.Name},{category.Type},{category.Description},{category.CreatedAt:yyyy-MM-dd HH:mm:ss}";
            await File.AppendAllTextAsync($"{_outputPath}/categories.csv", csvLine + "\n");
        }

        public async Task VisitOperation(Operation operation)
        {
            var csvLine = $"{operation.Id},{operation.AccountId},{operation.CategoryId},{operation.Amount},{operation.Description},{operation.Date:yyyy-MM-dd HH:mm:ss}";
            await File.AppendAllTextAsync($"{_outputPath}/operations.csv", csvLine + "\n");
        }
    }
} 