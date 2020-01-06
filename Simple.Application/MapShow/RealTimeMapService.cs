using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class RealTimeMapService : IRealTimeMapService
    {
        private IMapShowDomainService mapShowDomainService;

        public RealTimeMapService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }

        public async Task<List<CarExEntity>> GetCarExEntitiesByUser(int userId)
        {
            var cars = await mapShowDomainService.GetCarEntitiesByUser(userId);
            return cars;
        }

        public async Task<TreeNode[]> GetDeviceTreeByUser(int userId)
        {
            var nodes = await mapShowDomainService.GetDeviceTreeByUser(userId);
            return nodes;
        }

        public async Task<List<LastLocatedExEntity>> GetLastLocatedByUser(int userId)
        {
            var lastLocateds = await mapShowDomainService.GetLastLocatedByUser(userId);
            return lastLocateds;
        }

        public async Task<List<PersonExEntity>> GetPersonExEntitiesByUser(int userId)
        {
            var persons = await mapShowDomainService.GetPersonEntitiesByUser(userId);
            return persons;
        }

        public async Task<List<ViewAllTargetExEntity>> GetViewAllTargetByUser(int userId)
        {
            var devices = await mapShowDomainService.GetAllDeviceByUser(userId);
            return devices;
        }
    }
}
