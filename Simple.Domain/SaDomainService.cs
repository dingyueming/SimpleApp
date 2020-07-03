using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.ExEntity.Map;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Domain
{
    public class SaDomainService : ISaDomainService
    {
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly ICarRepository carRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        public SaDomainService(IUnitRepository unitRepository, ICarRepository carRepository, ILastLocatedRepository lastLocatedRepository, IMapper mapper)
        {
            this.unitRepository = unitRepository;
            this.carRepository = carRepository;
            this.lastLocatedRepository = lastLocatedRepository;
            this.mapper = mapper;
        }

        public async Task<Pagination<LastLocatedExEntity>> GetLastLocatedPage(Pagination<LastLocatedExEntity> param, DateTime[] dateTimes)
        {
            var list = await lastLocatedRepository.GetAllAsync();
            var unitEntities = await unitRepository.GetAllAsync();
            var unitExEntities = mapper.Map<List<UnitExEntity>>(unitEntities);
            var exList = mapper.Map<List<LastLocatedExEntity>>(list);
            if (param.SearchData.IsSearchLocated)
            {
                exList = mapper.Map<List<LastLocatedExEntity>>(list).Where(x => x.GNSSTIME >= dateTimes[0] && x.GNSSTIME <= dateTimes[1]).ToList();
            }
            else
            {
                exList = mapper.Map<List<LastLocatedExEntity>>(list).Where(x => x.GNSSTIME < dateTimes[0]).ToList();
            }
            var cars = await carRepository.GetAllAsync();
            exList.ForEach((x) =>
            {
                var carEntity = cars.FirstOrDefault(o => o.CARID == x.CARID);
                if (carEntity != null)
                {
                    x.Car = mapper.Map<ExEntity.Map.CarExEntity>(carEntity);
                    var unit = unitExEntities.FirstOrDefault(u => u.UNITID == carEntity.UNITID);
                    if (unit != null)
                    {
                        x.Unit = unit;
                    }
                }
            });
            param.Data = exList.ToList().Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();
            param.Total = exList.Count();
            return param;
        }
    }
}
