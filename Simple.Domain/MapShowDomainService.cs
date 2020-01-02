using Simple.Entity.GM;
using Simple.IDomain;
//using Simple.Infrastructure.InfrastructureModel.BootStrapTreeView;
using Simple.IRepository.GM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;

namespace Simple.Domain
{
    /// <summary>
    /// 地图显示domain
    /// </summary>
    public class MapShowDomainService : IMapShowDomainService
    {
        private readonly ICarRepository carRepository;
        private readonly IUnitRepository unitRepository;
        public MapShowDomainService(ICarRepository carRepository, IUnitRepository unitRepository)
        {
            this.carRepository = carRepository;
            this.unitRepository = unitRepository;
        }

        public async Task<TreeNode[]> GetDeviceTreeByUser(int userId)
        {
            //获取所有单位
            var allUnits = await unitRepository.GetAllAsync();
            //获取当前用户所拥有的设备车辆
            var cars = await carRepository.GetCarEntitiesByUser(userId);
            //组织tree
            //var userGroup = users.GroupBy(x => x.UNITID).ToList();
            var listNode = new List<TreeNode>();
            foreach (var unit in allUnits)
            {
                var treeNode = new TreeNode()
                {
                    name = unit.UNITNAME,
                    id = $"unit-{ unit.UNITID }",
                    pId = $"unit-{unit.PID}",
                    isParent = true,
                    iconSkin = "pIcon01"
                };
                listNode.Add(treeNode);
            }
            foreach (var car in cars)
            {
                var treeNode = new TreeNode()
                {
                    name = car.LICENSE,
                    id = $"user-{ car.CARID }",
                    pId = $"unit-{car.UNITID}",
                    iconSkin = "gray_car",
                };
                listNode.Add(treeNode);
            }

            //firstNode.nodes = GetUnitTreeViews(allUnits.ToList(), 1).ToArray();
            //treeView.data = new TreeNode[] { firstNode };
            return listNode.ToArray();
        }
        ///// <summary>
        ///// 递归单位树
        ///// </summary>
        ///// <returns></returns>
        //private List<TreeNode> GetUnitTreeViews(List<UnitEntity> allUnits, int parentId)
        //{
        //    var listTreeNode = new List<TreeNode>();

        //    var children = allUnits.Where(x => x.PID == parentId).ToList();
        //    if (children.Count > 0)
        //    {
        //        children.ForEach((x) =>
        //        {
        //            var nodeItme = new TreeNode
        //            {
        //                text = x.UNITNAME
        //            };
        //            listTreeNode.Add(nodeItme);
        //            nodeItme.nodes = GetUnitTreeViews(allUnits, x.UNITID).ToArray();
        //        });
        //    }
        //    return listTreeNode;
        //}
    }
}
