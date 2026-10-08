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

            if (category == null)
            {
                Console.WriteLine("Категорію з таким ID не знайдено.");
                return false; 
            }

            Console.Write($"Ви впевнені, що хочете видалити категорію '{category.CategoryName}' та всі пов'язані з нею товари? (Так/Ні): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "так" || confirmation == "т")
            {
                var relatedProducts = _context.Products.Where(p => p.CategoryId == id);
                _context.Products.RemoveRange(relatedProducts);
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                Console.WriteLine("Категорію та пов'язані товари успішно видалено!");
                return true; 
            }
            else
            {
                Console.WriteLine("Видалення скасовано користувачем.");
                return false; 
            }
        }
    }
}
    
    
