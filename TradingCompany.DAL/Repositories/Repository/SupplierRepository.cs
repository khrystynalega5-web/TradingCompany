using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Interface;

namespace TradingCompany.DAL.Repositories.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly TradingCompanyDbContext _context;

        public SupplierRepository(TradingCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task AddAsync(Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier entity)
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                Console.WriteLine("Постачальника з таким ID не знайдено.");
                return false;
            }

            Console.Write($"Ви впевнені, що хочете видалити постачальника '{supplier.CompanyName}' та всі пов'язані з ним товари? (Так/Ні): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "так" || confirmation == "т")
            {
                var relatedProducts = _context.Products.Where(p => p.SupplierId == id);
                _context.Products.RemoveRange(relatedProducts);
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
                Console.WriteLine("Постачальника та пов'язані товари успішно видалено!");
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