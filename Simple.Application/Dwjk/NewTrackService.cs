using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using System;
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

        public async Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword,DateTime startTime,DateTime endTime)
        {
            return await ifDomainService.GetHistoryTrackList(keyword,startTime,endTime);
        }
    }
}
