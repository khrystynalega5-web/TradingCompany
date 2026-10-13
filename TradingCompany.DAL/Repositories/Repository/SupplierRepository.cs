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
                return false;
            }

            var relatedProducts = _context.Products.Where(p => p.SupplierId == id).ToList();

            if (relatedProducts.Any())
            {
                _context.Products.RemoveRange(relatedProducts);
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return true;
        }
    }
    
}