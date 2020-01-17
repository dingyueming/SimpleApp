using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface IDmDomainService
    {
        #region 单位管理

        Task<bool> AddUnit(UnitExEntity exEntity);
        Task<bool> DeleteUnit(List<UnitExEntity> exEntities);
        Task<bool> UpdateUnit(UnitExEntity exEntity);
        Task<Pagination<UnitExEntity>> GetUnitPage(Pagination<UnitExEntity> param);
        Task<VueTreeSelectModel[]> GetUnitTree();

        #endregion

        #region 车辆管理

        Task<bool> AddCar(CarExEntity exEntity);
        Task<bool> DeleteCar(List<CarExEntity> exEntities);
        Task<bool> UpdateCar(CarExEntity exEntity);
        Task<Pagination<CarExEntity>> GetCarPage(Pagination<CarExEntity> param);

        #endregion

    }
}
