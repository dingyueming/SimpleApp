using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using Simple.Infrastructure.Tools;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain
{
    public class DmDomainService : IDmDomainService
    {
        #region 构造函数
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        public DmDomainService(IUnitRepository unitRepository, IMapper mapper)
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
        }
        #endregion

        #region 单位管理
        public async Task<bool> AddUnit(UnitExEntity exEntity)
        {
            var entity = mapper.Map<UnitEntity>(exEntity);
            return await unitRepository.InsertAsync(entity);
        }

        public async Task<bool> DeleteUnit(List<UnitExEntity> exEntities)
        {
            var entities = mapper.Map<List<UnitEntity>>(exEntities);
            return await unitRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdateUnit(UnitExEntity exEntity)
        {
            var entity = mapper.Map<UnitEntity>(exEntity);
            return await unitRepository.UpdateAsync(entity);
        }

        public async Task<Pagination<UnitExEntity>> GetUnitPage(Pagination<UnitExEntity> param)
        {
            var pagination = await unitRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<UnitExEntity>>(pagination);
        }
        public async Task<VueTreeSelectModel[]> GetUnitTree()
        {
            var allUnits = await unitRepository.GetAllAsync();
            var treeSelectModels = GetUnitTreeSelectModels(allUnits.ToList(), null);
            return treeSelectModels.ToArray();
        }
        #endregion
        #region 递归单位tree

        public static List<VueTreeSelectModel> GetUnitTreeSelectModels(List<UnitEntity> allNodes, UnitEntity node)
        {
            var list = new List<VueTreeSelectModel>();
            if (node == null)
            {
                var unit = allNodes.FirstOrDefault(x => x.PID == 0);
                var nodeChildren = GetUnitTreeSelectModels(allNodes, unit);
                var firstNode = new VueTreeSelectModel() { id = unit.UNITID.ToString(), label = unit.UNITNAME, Tag = unit.ORG_CODE, children = nodeChildren?.ToArray() };
                list.Add(firstNode);
            }
            else
            {
                var nodes = allNodes.Where(x => x.PID == node.UNITID).ToList();
                if (nodes.Count > 0)
                {
                    nodes.ForEach((x) =>
                    {
                        var nodeChildren = GetUnitTreeSelectModels(allNodes, x);
                        list.Add(new VueTreeSelectModel() { id = x.UNITID.ToString(), label = x.UNITNAME, Tag = x.ORG_CODE, children = nodeChildren?.ToArray() });
                    });
                }
                else
                {
                    return null;
                }
            }

            return list;
        }

        #endregion
    }
}
