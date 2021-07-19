using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IAsyncRepository<T> where T:class
    {
        //Common CRUD operation that will be used by all other repositories

        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter);
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
        Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null);
        //Task<T> AddAsync(T entity);
        //Task<T> UpdateAsync(T entity);
        //Task DeleteAsync(T entity);

    }
}
