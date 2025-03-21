using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Visitors
{
    public interface IDataVisitor
    {
        Task VisitBankAccount(BankAccount account);
        Task VisitCategory(Category category);
        Task VisitOperation(Operation operation);
    }
} 