using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using Simple.Infrastructure.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class SjtlAttendancePositionService : ISjtlAttendancePositionService
    {
        private IMapShowDomainService mapShowDomainService;
        public SjtlAttendancePositionService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }
        public async Task<List<SjtlAttendancePositionExEntity>> GetSjtlAttenPosExEntities(SjtlAttPosQm qm)
        {
            return await mapShowDomainService.GetSjtlAttenPosExEnties(qm);
        }
    }
}
