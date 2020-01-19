using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Element;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.IApplication.DM
{
    public interface IUnitService
    {
        Task<bool> Add(UnitExEntity exEntity);
        Task<bool> Delete(List<UnitExEntity> exEntities);
        Task<bool> Update(UnitExEntity exEntity);
        Task<Pagination<UnitExEntity>> GetPage(Pagination<UnitExEntity> param);
        Task<VueTreeSelectModel[]> GetUnitTree();
        Task<ElementTreeModel[]> GetUnitAndDeviceTree();
    }
}
