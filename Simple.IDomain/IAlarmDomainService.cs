using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
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
        Task AddCarArea(List<CarAreaExEntity> exEntities);
        Task DeleteCarArea(CarAreaExEntity exEntity);
        Task<AreaExEntity> GetAlarmArea(int areaId);
        Task<Pagination<NewAlarmInfoExEntity>> GetNewAlarmInfoPage(Pagination<NewAlarmInfoExEntity> param);

    }
}
