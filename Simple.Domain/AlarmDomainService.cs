using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Element;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using Simple.Infrastructure.Tools;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain
{
    public class AlarmDomainService : IAlarmDomainService
    {
        private readonly ICarAreaRepository carAreaRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IMapper mapper;
        public AlarmDomainService(ICarAreaRepository carAreaRepository, IAreaRepository areaRepository, IMapper mapper)
        {
            this.carAreaRepository = carAreaRepository;
            this.areaRepository = areaRepository;
            this.mapper = mapper;
        }
        public async Task<Pagination<AreaExEntity>> GetAlarmAreaPage(Pagination<AreaExEntity> param)
        {
            var pagination = await areaRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<AreaExEntity>>(pagination);
        }
        public async Task<bool> AddAreaAlarm(AreaExEntity exEntity)
        {
            var entity = mapper.Map<AreaEntity>(exEntity);
            //数据校验
            var valdataEntity = await areaRepository.GetEntityForValdata(entity);
            if (valdataEntity != null)
            {
                throw new Exception("重复的区域名称");
            }
            await areaRepository.InsertAlarmArea(entity);
            return true;
        }
        public async Task<bool> DeleteAreaAlarm(List<AreaExEntity> exEntities)
        {
            var entities = mapper.Map<List<AreaEntity>>(exEntities);
            return await areaRepository.DeleteAsync(entities);
        }
        public async Task<List<AreaExEntity>> GetAllAras()
        {
            var entities = await areaRepository.GetAllAsync();
            return mapper.Map<List<AreaExEntity>>(entities);
        }
        public async Task AddCarArea(CarAreaExEntity exEntity)
        {
            var entity = mapper.Map<CarAreaEntity>(exEntity);
            await carAreaRepository.InsertCarArea(entity);
        }
    }
}
