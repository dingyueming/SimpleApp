using Simple.ExEntity.Map;
using Simple.Infrastructure.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.MapShow
{
    public interface IMapSearchService
    {
        Task<List<XfKeyUnitExEntity>> GetXfKeyUnitExEntities();
    }
}
