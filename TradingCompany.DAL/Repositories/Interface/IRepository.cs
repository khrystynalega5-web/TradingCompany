using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingCompany.DAL.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task <bool>DeleteAsync(int id);
    }
}