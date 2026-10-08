using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Interface;

namespace TradingCompany.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TradingCompanyDbContext _context;

        public ProductRepository(TradingCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
         .Include(p => p.Category)
         .Include(p => p.Supplier)
         .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Supplier)
        .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                Console.WriteLine("Товар з таким ID не знайдено.");
                return false;
            }

            Console.Write($"Ви впевнені, що хочете видалити товар '{product.ProductName}' та всі пов'язані з ним продажі? (Так/Ні): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "так" || confirmation == "т")
            {
                var relatedSales = _context.Sales.Where(s => s.ProductId == id);
                _context.Sales.RemoveRange(relatedSales);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                Console.WriteLine("Товар та пов'язані продажі успішно видалено!");
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