
using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.SM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Domain
{
    /// <summary>
    /// 对外接口领域服务
    /// </summary>
    public class LogDomainService : ILogDomainService
    {
        private readonly IOperateLogRepository operateLogRepository;
        private readonly IMapper mapper;

        public LogDomainService(IOperateLogRepository operateLogRepository, IMapper mapper)
        {
            this.operateLogRepository = operateLogRepository;
            this.mapper = mapper;
        }
        #region 操作日志
        public async Task AddLog(OperateLogExEntity exEntity)
        {
            var entity = mapper.Map<OperateLogEntity>(exEntity);
            await operateLogRepository.InsertAsync(entity);
        }

        public Task DeleteOperateLog(List<OperateLogExEntity> exEntities)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<OperateLogExEntity>> GetLogPage(Pagination<OperateLogExEntity> param)
        {
            var pagination = await operateLogRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<OperateLogExEntity>>(pagination);
        }

        public Task UpdateOperateLog(OperateLogExEntity exEntity)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
