using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class CarController : SimpleBaseController
    {
        private readonly ICarService carService;
        public CarController(ICarService carService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.carService = carService;
        }
        public async Task<JsonResult> Query(Pagination<CarExEntity> pagination)
        {
            var data = await carService.GetPage(pagination);
            return Json(data);
        }

        public async Task<bool> Add(CarExEntity exEntity)
        {
            return await carService.Add(exEntity);
        }
        public async Task<bool> Update(CarExEntity exEntity)
        {
            exEntity.RECORDDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await carService.Update(exEntity);
        }
        public async Task<bool> Delete(CarExEntity exEntity)
        {
            return await carService.Delete(new List<CarExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<CarExEntity> exEntities)
        {
            return await carService.Delete(exEntities);
        }
    }
}