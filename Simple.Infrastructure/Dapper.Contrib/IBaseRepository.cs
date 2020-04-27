using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Infrastructure.Dapper.Contrib
{
    public interface IBaseRepository<T> where T : class
    {
        IDbConnection GetDbConnection();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIDAsync(object id);
        Task<bool> InsertAsync(T info);
        Task<bool> InsertAsync(IEnumerable<T> list);
        Task<bool> UpdateAsync(T info);
        Task<bool> UpdateAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(T info);
        Task<bool> DeleteAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int[] ids);
        Task<bool> DeleteAllAsync();
        Task<Pagination<T>> GetSingleTbPage(string tableName, int pageSize, int pageIndex, string where, string orderby);
    }
}
