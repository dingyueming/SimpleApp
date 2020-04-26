using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Threading.Tasks;

namespace Simple.IApplication.MapShow
{
    public interface INewAlarmInfoService
    {
        Task<Pagination<NewAlarmInfoExEntity>> GetPage(Pagination<NewAlarmInfoExEntity> param);
    }
}
