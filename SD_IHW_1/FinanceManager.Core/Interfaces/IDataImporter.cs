using System.Threading.Tasks;

namespace FinanceManager.Core.Interfaces
{
    public interface IDataImporter
    {
        Task<T> ImportAsync<T>(string filePath);
    }
} 