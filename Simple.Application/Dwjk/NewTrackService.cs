using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Application.Dwjk
{
    public class NewTrackService : INewTrackService
    {
        private readonly IIfDomainService ifDomainService;
        public NewTrackService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword)
        {
            return await ifDomainService.GetHistoryTrackList(keyword);
        }
    }
}
