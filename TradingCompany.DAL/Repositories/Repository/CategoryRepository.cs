using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Interface;

namespace TradingCompany.DAL.Repositories.Repository
{

    public class CategoryRepository : ICategoryRepository
    {
        private readonly TradingCompanyDbContext _context;

        public CategoryRepository(TradingCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category entity)
        {
            var exists = await _context.Categories.AnyAsync(c => c.CategoryName == entity.CategoryName);

            if (exists)
            {
                throw new InvalidOperationException($"Категорія з назвою '{entity.CategoryName}' вже існує.");
            }

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            var relatedProducts = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (relatedProducts.Any())
            {
                _context.Products.RemoveRange(relatedProducts);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

    
    
