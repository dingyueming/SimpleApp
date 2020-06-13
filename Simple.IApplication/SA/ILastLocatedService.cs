using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SA
{
    public interface ILastLocatedService
    {
        Task<Pagination<LastLocatedExEntity>> GetPage(Pagination<LastLocatedExEntity> param, DateTime[] dateTimes);
    }
}
