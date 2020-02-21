using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IRepository
{
    public interface IAreaRepository : IBaseRepository<AreaEntity>
    {
        Task<Pagination<AreaEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby);

        Task<AreaEntity> GetEntityForValdata(AreaEntity entity);

        Task InsertAlarmArea(AreaEntity entity);

    }
}
