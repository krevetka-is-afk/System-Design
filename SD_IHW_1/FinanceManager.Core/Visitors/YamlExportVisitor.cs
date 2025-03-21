using System.IO;
using System.Threading.Tasks;
using FinanceManager.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FinanceManager.Core.Visitors
{
    public class YamlExportVisitor : IDataVisitor
    {
        private readonly string _outputPath;
        private readonly ISerializer _serializer;

        public YamlExportVisitor(string outputPath)
        {
            _outputPath = outputPath;
            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public async Task VisitBankAccount(BankAccount account)
        {
            var yaml = _serializer.Serialize(account);
            await File.WriteAllTextAsync($"{_outputPath}/account_{account.Id}.yaml", yaml);
        }

        public async Task VisitCategory(Category category)
        {
            var yaml = _serializer.Serialize(category);
            await File.WriteAllTextAsync($"{_outputPath}/category_{category.Id}.yaml", yaml);
        }

        public async Task VisitOperation(Operation operation)
        {
            var yaml = _serializer.Serialize(operation);
            await File.WriteAllTextAsync($"{_outputPath}/operation_{operation.Id}.yaml", yaml);
        }
    }
} 