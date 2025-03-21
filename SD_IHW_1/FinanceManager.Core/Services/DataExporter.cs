using System;
using System.IO;
using System.Threading.Tasks;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Visitors;

namespace FinanceManager.Core.Services
{
    public class DataExporter : IDataExporter
    {
        private readonly IFinanceService _financeService;

        public DataExporter(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task ExportAsync(string outputPath, string format)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            IDataVisitor visitor = format.ToLower() switch
            {
                "json" => new JsonExportVisitor(outputPath),
                "csv" => new CsvExportVisitor(outputPath),
                "yaml" => new YamlExportVisitor(outputPath),
                _ => throw new ArgumentException($"Unsupported format: {format}")
            };

            var accounts = await _financeService.GetAllAccountsAsync();
            var categories = await _financeService.GetAllCategoriesAsync();
            var operations = await _financeService.GetAllOperationsAsync();

            foreach (var account in accounts)
            {
                await visitor.VisitBankAccount(account);
            }

            foreach (var category in categories)
            {
                await visitor.VisitCategory(category);
            }

            foreach (var operation in operations)
            {
                await visitor.VisitOperation(operation);
            }
        }
    }
} 