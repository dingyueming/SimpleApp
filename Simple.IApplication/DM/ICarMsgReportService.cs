using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.DM
{
    public interface ICarMsgReportService
    {
        Task<bool> Add(CarMsgReportExEntity exEntity);
        Task<bool> Delete(List<CarMsgReportExEntity> exEntities);
        Task<bool> Update(CarMsgReportExEntity exEntity);
        Task<Pagination<CarMsgReportExEntity>> GetPage(Pagination<CarMsgReportExEntity> param);
        Task<List<CarMsgReportExEntity>> GetCarMsgReportExEntities(DateTime[] dateTimes);
    }
}
