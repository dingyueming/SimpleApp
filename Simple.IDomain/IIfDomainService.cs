using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface IIfDomainService
    {
        Task<LastLocatedExEntity> GetLastLocatedByMac(string mac);

    }
}
