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
    public class CarMsgReportService : ICarMsgReportService
    {
        private readonly IDmDomainService dmDomainService;
        public CarMsgReportService(IDmDomainService dmDomainService)
        {
            this.dmDomainService = dmDomainService;
        }

        public async Task<bool> Add(CarMsgReportExEntity exEntity)
        {
            return await dmDomainService.AddCarMsgReport(exEntity);
        }

        public async Task<bool> Delete(List<CarMsgReportExEntity> exEntities)
        {
            return await dmDomainService.DeleteCarMsgReport(exEntities);
        }

        public async Task<CarMsgReportExEntity> GetCarMsgReportExEntity(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<CarMsgReportExEntity>> GetPage(Pagination<CarMsgReportExEntity> param)
        {
            return await dmDomainService.GetCarMsgReportPage(param);
        }

        public async Task<bool> Update(CarMsgReportExEntity exEntity)
        {
            return await dmDomainService.UpdateCarMsgReport(exEntity);
        }
    }
}
