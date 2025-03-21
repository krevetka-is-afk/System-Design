using System.Threading.Tasks;

namespace FinanceManager.Core.Interfaces
{
    public interface IDataExporter
    {
        Task ExportAsync(string outputPath, string format);
    }
} 