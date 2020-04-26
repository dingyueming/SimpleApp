using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class NewAlarmInfoService : INewAlarmInfoService
    {
        private IAlarmDomainService alarmDomainService;
        public NewAlarmInfoService(IAlarmDomainService alarmDomainService)
        {
            this.alarmDomainService = alarmDomainService;
        }
        public async Task<Pagination<NewAlarmInfoExEntity>> GetPage(Pagination<NewAlarmInfoExEntity> param)
        {
            return await alarmDomainService.GetNewAlarmInfoPage(param);
        }
    }
}
