using System.Text.Json;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Visitors
{
    public class JsonExportVisitor : IDataVisitor
    {
        private readonly JsonSerializerOptions _options;
        private readonly string _outputPath;

        public JsonExportVisitor(string outputPath)
        {
            _outputPath = outputPath;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public async Task VisitBankAccount(BankAccount account)
        {
            var json = JsonSerializer.Serialize(account, _options);
            await System.IO.File.WriteAllTextAsync($"{_outputPath}/account_{account.Id}.json", json);
        }

        public async Task VisitCategory(Category category)
        {
            var json = JsonSerializer.Serialize(category, _options);
            await System.IO.File.WriteAllTextAsync($"{_outputPath}/category_{category.Id}.json", json);
        }

        public async Task VisitOperation(Operation operation)
        {
            var json = JsonSerializer.Serialize(operation, _options);
            await System.IO.File.WriteAllTextAsync($"{_outputPath}/operation_{operation.Id}.json", json);
        }
    }
} 