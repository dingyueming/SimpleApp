using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.DM
{
    public class AreaAlarmService : IAreaAlarmService
    {
        private readonly IAlarmDomainService alarmDomainService;
        public AreaAlarmService(IAlarmDomainService alarmDomainService)
        {
            this.alarmDomainService = alarmDomainService;
        }
        public async Task<bool> AddAreaAlarm(AreaExEntity exEntity)
        {
            return await alarmDomainService.AddAreaAlarm(exEntity);
        }
        public async Task<bool> DeleteAreaAlarm(List<AreaExEntity> exEntities)
        {
            return await alarmDomainService.DeleteAreaAlarm(exEntities);
        }

        public async Task<Pagination<AreaExEntity>> GetPage(Pagination<AreaExEntity> param)
        {
            return await alarmDomainService.GetAlarmAreaPage(param);
        }

        public async Task<List<AreaExEntity>> GetAllAras()
        {
            return await alarmDomainService.GetAllAras();
        }

        public async Task AddCarArea(CarAreaExEntity exEntity)
        {
            await alarmDomainService.AddCarArea(exEntity);
        }

        public async Task AddCarArea(List<CarAreaExEntity> exEntities)
        {
            await alarmDomainService.AddCarArea(exEntities);
        }

        public async Task DeleteCarArea(CarAreaExEntity exEntity)
        {
            await alarmDomainService.DeleteCarArea(exEntity);
        }

        public async Task<AreaExEntity> GetAreaExEntity(int areaId)
        {
            return await alarmDomainService.GetAlarmArea(areaId);
        }
    }
}
