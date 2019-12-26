using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Infrastructure.Dapper.Contrib
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIDAsync(object id);
        Task<bool> InsertAsync(T info);
        Task<bool> InsertAsync(IEnumerable<T> list);
        Task<bool> UpdateAsync(T info);
        Task<bool> UpdateAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(T info);
        Task<bool> DeleteAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(object id);
        Task<bool> DeleteAllAsync();
    }
}
