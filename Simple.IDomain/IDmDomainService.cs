using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.Element;

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
        Task<ElementTreeModel[]> GetUnitAndDeviceTree();
        Task<List<UnitExEntity>> GetAllUnitExEntities();


        #endregion

        #region 车辆管理

        Task<bool> AddCar(CarExEntity exEntity);
        Task<bool> DeleteCar(List<CarExEntity> exEntities);
        Task<bool> UpdateCar(CarExEntity exEntity);
        Task<Pagination<CarExEntity>> GetCarPage(Pagination<CarExEntity> param);
        Task<List<CarExEntity>> GetAllCarExEntities();

        #endregion

        #region 人员对讲机管理

        Task<bool> AddPerson(PersonExEntity exEntity);
        Task<bool> DeletePerson(List<PersonExEntity> exEntities);
        Task<bool> UpdatePerson(PersonExEntity exEntity);
        Task<Pagination<PersonExEntity>> GetPersonPage(Pagination<PersonExEntity> param);

        #endregion

        #region 设备分配

        Task<string[]> GetDeviceIdsByUser(int userId);
        Task<bool> UpdateAuthLimits(List<ElementTreeModel> nodes, int userId);
        #endregion

        #region 车辆报备

        Task<bool> AddCarMsgReport(CarMsgReportExEntity exEntity);
        Task<bool> DeleteCarMsgReport(List<CarMsgReportExEntity> exEntities);
        Task<bool> UpdateCarMsgReport(CarMsgReportExEntity exEntity);
        Task<Pagination<CarMsgReportExEntity>> GetCarMsgReportPage(Pagination<CarMsgReportExEntity> param);
        Task<List<CarMsgReportExEntity>> GetCarMsgReportExEntities(DateTime[] dateTimes, string carNo);


        #endregion

    }
}
