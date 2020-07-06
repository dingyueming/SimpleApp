using Simple.ExEntity.Map;
using Simple.ExEntity.SA;
using Simple.IApplication.SA;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Threading.Tasks;

namespace Simple.Application.SA
{
    public class AlarmRecordService : IAlarmRecordService
    {
        private readonly ISaDomainService saDomainService;
        public AlarmRecordService(ISaDomainService saDomainService)
        {
            this.saDomainService = saDomainService;
        }

        public async Task<Pagination<AlarmRecordExEntity>> GetPage(Pagination<AlarmRecordExEntity> param)
        {
            var page = await saDomainService.GetAlarmRecordPage(param);
            return page;
        }
    }
}
