using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Dapper;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IRepository
{
    public interface IInterfaceLogRepository : IBaseRepository<InterfaceLogEntity>
    {
        Task<Pagination<InterfaceLogEntity>> GetPage(int pageSize, int pageIndex, string orderby, string where);

    }
}
