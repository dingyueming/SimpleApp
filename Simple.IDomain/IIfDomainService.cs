using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface IIfDomainService
    {
        Task<LastLocatedExEntity> GetLastLocatedByMac(string mac);

        Task<LastLocatedExEntity> GetLastLocated(string keyword);

        Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword, DateTime startTime, DateTime endTime);

        Task<ViewAllTargetExEntity> GetViewAllTarget(string keyword);

        Task<List<ViewAllTargetExEntity>> GetViewAllTargetListByOrgcode(string code);

        Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime);
    }
}
