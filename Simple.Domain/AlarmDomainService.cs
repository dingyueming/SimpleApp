using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Domain
{
    public class AlarmDomainService : IAlarmDomainService
    {
        private readonly INewAlarmInfoRepository newAlarmInfoRepository;
        private readonly ICarAreaRepository carAreaRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IMapper mapper;
        public AlarmDomainService(INewAlarmInfoRepository newAlarmInfoRepository, ICarAreaRepository carAreaRepository, IAreaRepository areaRepository, IMapper mapper)
        {
            this.newAlarmInfoRepository = newAlarmInfoRepository;
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
        public async Task AddCarArea(List<CarAreaExEntity> exEntities)
        {
            var entities = mapper.Map<List<CarAreaEntity>>(exEntities);
            await carAreaRepository.BatchInsertCarArea(entities);
        }
        public async Task DeleteCarArea(CarAreaExEntity exEntity)
        {
            var entity = mapper.Map<CarAreaEntity>(exEntity);
            await carAreaRepository.DeleteAsync(entity);
        }
        public async Task<AreaExEntity> GetAlarmArea(int areaId)
        {
            var entity = await areaRepository.GetAreaEntityById(areaId);
            return mapper.Map<AreaExEntity>(entity);
        }

        public async Task<Pagination<NewAlarmInfoExEntity>> GetNewAlarmInfoPage(Pagination<NewAlarmInfoExEntity> param)
        {
            if (param.SearchData.DateTimes != null)
            {
                param.Where = $" and a.gnsstime between to_date('{param.SearchData.DateTimes[0]}','yyyy-mm-dd hh24:mi:ss') and to_date('{param.SearchData.DateTimes[1]}','yyyy-mm-dd hh24:mi:ss')";
                param.Where += $"and b.license like '%{param.SearchData.License}%'";
            }
            var pagination = await newAlarmInfoRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<NewAlarmInfoExEntity>>(pagination);
        }


    }
}
