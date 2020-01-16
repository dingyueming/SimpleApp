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
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllAsync();
        /// <summary>
        /// dapper通用分页方法
        /// </summary>
        /// <typeparam name="T">泛型集合实体类</typeparam>
        /// <param name="files">列</param>
        /// <param name="tableName">表</param>
        /// <param name="where">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页显示条数</param>
        /// <param name="total">结果集总数</param>
        /// <returns></returns>
       IEnumerable<T> GetPageList(string files, string tableName, string where, string orderby, int pageIndex, int pageSize, out int total);
    }
}
