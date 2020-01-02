using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;

namespace Simple.IDomain
{
    /// <summary>
    /// 地图显示domain接口
    /// </summary>
    public interface IMapShowDomainService
    {
        Task<TreeNode[]> GetDeviceTreeByUser(int userId);
    }
}
