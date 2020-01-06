using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.ZTree;

namespace Simple.IDomain
{
    /// <summary>
    /// 地图显示domain接口
    /// </summary>
    public interface IMapShowDomainService
    {
        Task<TreeNode[]> GetDeviceTreeByUser(int userId);

        Task<List<CarExEntity>> GetCarEntitiesByUser(int userId);

        Task<List<PersonExEntity>> GetPersonEntitiesByUser(int userId);

        Task<List<LastLocatedExEntity>> GetLastLocatedByUser(int userId);

        Task<List<ViewAllTargetExEntity>> GetAllDeviceByUser(int userId);

    }
}
