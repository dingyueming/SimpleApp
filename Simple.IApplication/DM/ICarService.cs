using Simple.ExEntity.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.DM
{
    public interface ICarService
    {
        Task<bool> Add(CarExEntity exEntity);
        Task<bool> Delete(List<CarExEntity> exEntities);
        Task<bool> Update(CarExEntity exEntity);
        Task<Pagination<CarExEntity>> GetPage(Pagination<CarExEntity> param);
        Task<List<CarExEntity>> GetAll();
    }
}
