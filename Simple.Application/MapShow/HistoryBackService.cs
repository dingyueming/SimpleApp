using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class HistoryBackService : IHistoryBackService
    {
        private IMapShowDomainService mapShowDomainService;
        public HistoryBackService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }

        public async Task<List<NewTrackExEntity>> GetNewTrackList(dynamic queryModel)
        {
            var list = await mapShowDomainService.GetNewTrackList(queryModel);
            return list;
        }

        public async Task<VueTreeSelectModel[]> GetVueTreeSelectModels(int userId)
        {
            var arr = await mapShowDomainService.GetVueDeviceTreeByUser(userId);
            return arr;
        }

    }
}
