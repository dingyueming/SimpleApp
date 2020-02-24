using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface IAlarmDomainService
    {
        Task<Pagination<AreaExEntity>> GetAlarmAreaPage(Pagination<AreaExEntity> param);
        Task<bool> AddAreaAlarm(AreaExEntity exEntity);
        Task<bool> DeleteAreaAlarm(List<AreaExEntity> exEntities);
        Task<List<AreaExEntity>> GetAllAras();
        Task AddCarArea(CarAreaExEntity exEntity);
        Task DeleteCarArea(CarAreaExEntity exEntity);
    }
}
