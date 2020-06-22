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
    public interface ICarMsgReportRepository : IBaseRepository<CarMsgReportEntity>
    {
        Task<Pagination<CarMsgReportEntity>> GetPage(int pageSize, int pageIndex, string orderby, string where);

        Task<List<CarMsgReportEntity>> GetEntities(DateTime[] dateTimes, string carNo);
    }
}
