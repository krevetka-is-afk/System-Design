using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type);
    }
} 