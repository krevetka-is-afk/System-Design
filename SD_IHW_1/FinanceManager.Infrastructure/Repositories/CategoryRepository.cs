using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Models;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(FinanceDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added category: {CategoryName}", entity.Name);
            return entity;
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated category: {CategoryName}", entity.Name);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted category: {CategoryName}", category.Name);
            }
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type)
        {
            return await _context.Categories
                .Where(c => c.Type == type)
                .ToListAsync();
        }
    }
} 