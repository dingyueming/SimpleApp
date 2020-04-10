
using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.IDomain;
using Simple.Infrastructure.Tools;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain
{
    /// <summary>
    /// 对外接口领域服务
    /// </summary>
    public class IfDomainService : IIfDomainService
    {
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly IMapper mapper;

        public IfDomainService(ILastLocatedRepository lastLocatedRepository, IMapper mapper)
        {
            this.lastLocatedRepository = lastLocatedRepository;
            this.mapper = mapper;
        }

        public async Task<LastLocatedExEntity> GetLastLocatedByMac(string mac)
        {
            var entity = await lastLocatedRepository.GetEntityByMac(mac);
            return mapper.Map<LastLocatedExEntity>(entity);
        }

    }
}
