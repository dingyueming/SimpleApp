using Simple.ExEntity.Map;
using Simple.Infrastructure.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.MapShow
{
    public interface ISjgx110AlarmService
    {
        Task<List<Sjgx110AlarmExEntity>> GetSjgx110AlarmExEntities(Sjgx110AlarmQm queryModel);
    }
}
