using System.Threading.Tasks;

namespace FinanceManager.Core.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }

    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }
} 