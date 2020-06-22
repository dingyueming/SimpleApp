using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.DM
{
    public class CarService : ICarService
    {
        private readonly IDmDomainService dmDomainService;
        public CarService(IDmDomainService dmDomainService)
        {
            this.dmDomainService = dmDomainService;
        }
        public async Task<bool> Add(CarExEntity exEntity)
        {
            return await dmDomainService.AddCar(exEntity);
        }

        public async Task<bool> Delete(List<CarExEntity> exEntities)
        {
            return await dmDomainService.DeleteCar(exEntities);
        }

        public async Task<bool> Update(CarExEntity exEntity)
        {
            return await dmDomainService.UpdateCar(exEntity);
        }

        public async Task<Pagination<CarExEntity>> GetPage(Pagination<CarExEntity> param)
        {
            return await dmDomainService.GetCarPage(param);
        }

        public async Task<List<CarExEntity>> GetAll()
        {
            return await dmDomainService.GetAllCarExEntities();
        }
    }
}
