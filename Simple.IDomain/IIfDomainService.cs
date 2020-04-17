using Simple.ExEntity.DM;
using Simple.ExEntity.IM;
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
        #region 接口
        Task<LastLocatedExEntity> GetLastLocatedByMac(string mac);

        Task<LastLocatedExEntity> GetLastLocated(string keyword);

        Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword, DateTime startTime, DateTime endTime);

        Task<ViewAllTargetExEntity> GetViewAllTarget(string keyword);

        Task<List<ViewAllTargetExEntity>> GetViewAllTargetListByOrgcode(string code);

        Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime);
        #endregion

        #region 接口信息管理

        Task AddInterface(InterfaceExEntity exEntity);
        Task DeleteInterface(List<InterfaceExEntity> exEntities);
        Task UpdateInterface(InterfaceExEntity exEntity);
        Task<Pagination<InterfaceExEntity>> GetInterfacePage(Pagination<InterfaceExEntity> param);

        #endregion

    }
}
