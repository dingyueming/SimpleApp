
using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.ExEntity.IM;
using Simple.ExEntity.Map;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.Tools;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain
{
    /// <summary>
    /// 对外接口领域服务
    /// </summary>
    public class IfDomainService : IIfDomainService
    {
        private readonly IInterfaceLogRepository interfaceLogRepository;
        private readonly IInterfaceRepository interfaceRepository;
        private readonly ISjgx110AlarmRepository sjgx110AlarmRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IViewAllTargetRepository viewAllTargetRepository;
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly INewtrackRepository newtrackRepository;
        private readonly IMapper mapper;

        public IfDomainService(IInterfaceRepository interfaceRepository, IInterfaceLogRepository interfaceLogRepository,
        ISjgx110AlarmRepository sjgx110AlarmRepository, IUnitRepository unitRepository, IViewAllTargetRepository viewAllTargetRepository,
            INewtrackRepository newtrackRepository, ILastLocatedRepository lastLocatedRepository, IMapper mapper)
        {
            this.interfaceLogRepository = interfaceLogRepository;
            this.interfaceRepository = interfaceRepository;
            this.sjgx110AlarmRepository = sjgx110AlarmRepository;
            this.unitRepository = unitRepository;
            this.viewAllTargetRepository = viewAllTargetRepository;
            this.newtrackRepository = newtrackRepository;
            this.lastLocatedRepository = lastLocatedRepository;
            this.mapper = mapper;
        }

        #region 接口
        public async Task<LastLocatedExEntity> GetLastLocatedByMac(string mac)
        {
            var entity = await lastLocatedRepository.GetEntityByMac(mac);
            return mapper.Map<LastLocatedExEntity>(entity);
        }

        public async Task<LastLocatedExEntity> GetLastLocated(string keyword)
        {
            var entity = await lastLocatedRepository.GetEntityByKeyword(keyword);
            return mapper.Map<LastLocatedExEntity>(entity);
        }

        public async Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword, DateTime startTime, DateTime endTime)
        {
            var entities = await newtrackRepository.GetNewTrackEntities(keyword, startTime, endTime);
            return mapper.Map<List<NewTrackExEntity>>(entities);
        }

        public async Task<ViewAllTargetExEntity> GetViewAllTarget(string keyword)
        {
            var entity = await viewAllTargetRepository.GetViewAllTargetByKeyword(keyword);
            return mapper.Map<ViewAllTargetExEntity>(entity);
        }

        public async Task<List<ViewAllTargetExEntity>> GetViewAllTargetListByOrgcode(string code)
        {
            var allUnit = await unitRepository.GetAllAsync();
            var thisUnits = GetUnits(code, allUnit.ToList());
            var list = await viewAllTargetRepository.GetViewAllTarget(thisUnits.Select(x => x.UNITID.ToString()).ToArray());
            return mapper.Map<List<ViewAllTargetExEntity>>(list);
        }

        public async Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime)
        {
            var list = await sjgx110AlarmRepository.GetAlarmEntities(startTime, endTime, null, null);
            return mapper.Map<List<Sjgx110AlarmExEntity>>(list);
        }
        #endregion

        #region 接口信息管理

        public async Task AddInterface(InterfaceExEntity exEntity)
        {
            var entity = mapper.Map<InterfaceEntity>(exEntity);
            await interfaceRepository.InsertAsync(entity);
        }
        public async Task DeleteInterface(List<InterfaceExEntity> exEntities)
        {
            var entities = mapper.Map<List<InterfaceEntity>>(exEntities);
            await interfaceRepository.DeleteAsync(entities);
        }
        public async Task UpdateInterface(InterfaceExEntity exEntity)
        {
            var entity = mapper.Map<InterfaceEntity>(exEntity);
            await interfaceRepository.UpdateAsync(entity);
        }
        public async Task<Pagination<InterfaceExEntity>> GetInterfacePage(Pagination<InterfaceExEntity> param)
        {
            var pagination = await interfaceRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<InterfaceExEntity>>(pagination);
        }

        #endregion

        #region 接口日志管理

        public async Task<Pagination<InterfaceLogExEntity>> GetInterfaceLogPage(Pagination<InterfaceLogExEntity> param)
        {
            if (param.SearchData.DateTimes != null)
            {
                param.Where = $" and a.stat_time between to_date('{param.SearchData.DateTimes[0]}','yyyy-mm-dd hh24:mi:ss') and to_date('{param.SearchData.DateTimes[1]}','yyyy-mm-dd hh24:mi:ss')";
            }
            var pagination = await interfaceLogRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<InterfaceLogExEntity>>(pagination);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 递归获取某个组织机构下的所有单位
        /// </summary>
        /// <returns></returns>
        private List<UnitEntity> GetUnits(string code, List<UnitEntity> allUnit)
        {
            var list = new List<UnitEntity>();
            var entity = allUnit.FirstOrDefault(x => x.UNITID.ToString() == code);
            if (entity != null)
            {
                list.Add(entity);
                var children = allUnit.Where(x => x.PID == entity.UNITID).ToList();
                foreach (var item in children)
                {
                    list.AddRange(GetUnits(item.UNITID.ToString(), allUnit));
                }
            }
            return list;
        }


        #endregion
    }
}
