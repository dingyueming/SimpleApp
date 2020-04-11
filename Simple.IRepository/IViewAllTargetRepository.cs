using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface IViewAllTargetRepository : IBaseRepository<ViewAllTargetEntity>
    {
        Task<List<ViewAllTargetEntity>> GetDevicesByUser(int userId);
        Task<List<ViewAllTargetEntity>> GetAllDevice();
        Task<ViewAllTargetEntity> GetViewAllTargetByKeyword(string keyword);
        Task<List<ViewAllTargetEntity>> GetViewAllTarget(string[] orgCodes);
    }
}
