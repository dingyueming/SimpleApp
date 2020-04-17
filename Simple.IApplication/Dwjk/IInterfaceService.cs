using Simple.ExEntity.IM;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface IInterfaceService
    {
        Task Add(InterfaceExEntity exEntity);
        Task Delete(List<InterfaceExEntity> exEntities);
        Task Update(InterfaceExEntity exEntity);
        Task<Pagination<InterfaceExEntity>> GetPage(Pagination<InterfaceExEntity> param);
    }
}
