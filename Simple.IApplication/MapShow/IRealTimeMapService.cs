using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using Simple.ExEntity.Map;

namespace Simple.IApplication.MapShow
{
    public interface IRealTimeMapService
    {
        /// <summary>
        /// 获取设备列表tree
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<TreeNode[]> GetDeviceTreeByUser(int userId);

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CarExEntity>> GetCarExEntitiesByUser(int userId);

        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<PersonExEntity>> GetPersonExEntitiesByUser(int userId);
        /// <summary>
        /// 获取最后一次的定位数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<LastLocatedExEntity>> GetLastLocatedByUser(int userId);
        /// <summary>
        /// 获取车辆人员对讲机视图数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ViewAllTargetExEntity>> GetViewAllTargetByUser(int userId);

    }
}
