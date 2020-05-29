﻿using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using Simple.Infrastructure.QueryModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    /// <summary>
    /// 地图显示domain接口
    /// </summary>
    public interface IMapShowDomainService
    {
        Task<TreeNode[]> GetDeviceTreeByUser(int userId);

        Task<VueTreeSelectModel[]> GetVueDeviceTreeByUser(int userId);

        Task<List<CarExEntity>> GetCarEntitiesByUser(int userId);

        Task<List<PersonExEntity>> GetPersonEntitiesByUser(int userId);

        Task<List<LastLocatedExEntity>> GetLastLocatedByUser(int userId);

        Task<List<ViewAllTargetExEntity>> GetAllDeviceByUser(int userId);

        Task<List<NewTrackExEntity>> GetNewTrackList(dynamic queryModel);

        Task<List<Sjgx110AlarmExEntity>> GetSjgx110AlarmExEntities(Sjgx110AlarmQm qm);

        Task<List<SjtlAttendancePositionExEntity>> GetSjtlAttenPosExEnties(SjtlAttPosQm qm);

        Task<List<XfKeyUnitExEntity>> GetXfKeyUnitExEntities();
    }
}
