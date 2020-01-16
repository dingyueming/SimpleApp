using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.DM
{
    public class UnitService : IUnitService
    {
        private readonly IDmDomainService dmDomainService;
        public UnitService(IDmDomainService dmDomainService)
        {
            this.dmDomainService = dmDomainService;
        }
        public async Task<bool> Add(UnitExEntity exEntity)
        {
            return await dmDomainService.AddUnit(exEntity);
        }

        public async Task<bool> Delete(List<UnitExEntity> exEntities)
        {
            return await dmDomainService.DeleteUnit(exEntities);
        }

        public async Task<bool> Update(UnitExEntity exEntity)
        {
            return await dmDomainService.UpdateUnit(exEntity);
        }

        public async Task<Pagination<UnitExEntity>> GetPage(Pagination<UnitExEntity> param)
        {
            return await dmDomainService.GetUnitPage(param);
        }

        public async Task<VueTreeSelectModel[]> GetUnitTree()
        {
            return await dmDomainService.GetUnitTree();
        }
    }
}
