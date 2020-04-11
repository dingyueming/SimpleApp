
using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.IDomain;
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
        private readonly ISjgx110AlarmRepository sjgx110AlarmRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IViewAllTargetRepository viewAllTargetRepository;
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly INewtrackRepository newtrackRepository;
        private readonly IMapper mapper;

        public IfDomainService(ISjgx110AlarmRepository sjgx110AlarmRepository, IUnitRepository unitRepository, IViewAllTargetRepository viewAllTargetRepository,
            INewtrackRepository newtrackRepository, ILastLocatedRepository lastLocatedRepository, IMapper mapper)
        {
            this.sjgx110AlarmRepository = sjgx110AlarmRepository;
            this.unitRepository = unitRepository;
            this.viewAllTargetRepository = viewAllTargetRepository;
            this.newtrackRepository = newtrackRepository;
            this.lastLocatedRepository = lastLocatedRepository;
            this.mapper = mapper;
        }

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

        public async Task<List<NewTrackExEntity>> GetHistoryTrackList(string keyword)
        {
            var entities = await newtrackRepository.GetNewTrackEntities(keyword);
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
            var list = await viewAllTargetRepository.GetViewAllTarget(thisUnits.Select(x => x.ORG_CODE).ToArray());
            return mapper.Map<List<ViewAllTargetExEntity>>(list);
        }

        public async Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime)
        {
            var list = await sjgx110AlarmRepository.GetAlarmEntities(startTime, endTime, null, null);
            return mapper.Map<List<Sjgx110AlarmExEntity>>(list);
        }

        #region 私有方法

        /// <summary>
        /// 递归获取某个组织机构下的所有单位
        /// </summary>
        /// <returns></returns>
        private List<UnitEntity> GetUnits(string code, List<UnitEntity> allUnit)
        {
            var list = new List<UnitEntity>();
            var entity = allUnit.FirstOrDefault(x => x.ORG_CODE == code);
            if (entity != null)
            {
                list.Add(entity);
                var children = allUnit.Where(x => x.P_ORG_CODE == entity.ORG_CODE).ToList();
                foreach (var item in children)
                {
                    list.AddRange(GetUnits(item.ORG_CODE, allUnit));
                }
            }
            return list;
        }


        #endregion
    }
}
