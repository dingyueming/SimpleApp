using Simple.ExEntity.IM;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface IInterfaceLogService
    {
        Task<Pagination<InterfaceLogExEntity>> GetPage(Pagination<InterfaceLogExEntity> param);
    }
}
