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

        public async Task<TreeNode[]> GetDeviceTreeByUser(int userId)
        {
            var nodes = await mapShowDomainService.GetDeviceTreeByUser(userId);
            return nodes;
        }
    }
}
