using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;

namespace Simple.IApplication.MapShow
{
    public interface IRealTimeMapService
    {
        Task<TreeNode[]> GetDeviceTreeByUser(int userId);
    }
}
