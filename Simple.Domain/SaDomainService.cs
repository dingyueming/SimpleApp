using AutoMapper;
using Simple.Entity;
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
        private readonly IMapper mapper;
        public SaDomainService(ICarRepository carRepository, ILastLocatedRepository lastLocatedRepository, IMapper mapper)
        {
            this.carRepository = carRepository;
            this.lastLocatedRepository = lastLocatedRepository;
            this.mapper = mapper;
        }

        public async Task<Pagination<LastLocatedExEntity>> GetLastLocatedPage(Pagination<LastLocatedExEntity> param, DateTime[] dateTimes)
        {
            var list = await lastLocatedRepository.GetAllAsync();
            var exList = mapper.Map<List<LastLocatedExEntity>>(list);
            if (dateTimes.Length == 2)
            {
                 exList = mapper.Map<List<LastLocatedExEntity>>(list).Where(x => x.GNSSTIME > dateTimes[0] && x.GNSSTIME < dateTimes[1]).ToList();
            }
            var cars = await carRepository.GetAllAsync();
            exList.ForEach((x) =>
            {
                var carEntity = cars.FirstOrDefault(o => o.CARID == x.CARID);
                if (carEntity != null)
                {
                    x.Car = mapper.Map<CarExEntity>(carEntity);
                }
            });
            param.Data = exList.ToList().Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();
            param.Total = list.Count();
            return param;
        }
    }
}
