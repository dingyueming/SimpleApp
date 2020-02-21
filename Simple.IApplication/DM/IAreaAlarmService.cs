using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.DM
{
    public interface IAreaAlarmService
    {
        Task<bool> AddAreaAlarm(AreaExEntity exEntity);
        Task<bool> DeleteAreaAlarm(List<AreaExEntity> exEntities);
        Task<Pagination<AreaExEntity>> GetPage(Pagination<AreaExEntity> param);
        Task<List<AreaExEntity>> GetAllAras();
        Task AddCarArea(CarAreaExEntity exEntity);
    }
}
