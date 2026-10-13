using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories.Interface;

namespace TradingCompany.DAL.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly TradingCompanyDbContext _context;

        public SaleRepository(TradingCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
        .Include(s => s.Product)
        .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _context.Sales
        .Include(s => s.Product)
        .FirstOrDefaultAsync(s => s.SaleId == id);
        }

        public async Task AddAsync(Sale entity)
        {
            await _context.Sales.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale entity)
        {
            _context.Sales.Update(entity);
            _context.Entry(entity).Property(s => s.TotalSum).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                return false;
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return true;
        }
    }
        
}