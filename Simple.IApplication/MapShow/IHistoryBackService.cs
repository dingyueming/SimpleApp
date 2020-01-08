using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.ExEntity.Map;

namespace Simple.IApplication.MapShow
{
    public interface IHistoryBackService
    {
        Task<VueTreeSelectModel[]> GetVueTreeSelectModels(int userId);

        Task<List<NewTrackExEntity>> GetNewTrackList(dynamic queryModel);
    }
}
