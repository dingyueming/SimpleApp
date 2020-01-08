using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Infrastructure.Tools
{
    public static class TreeHelper
    {
        public static List<VueTreeSelectModel> GetTreeSelectModels(List<TreeNode> allNodes, TreeNode node)
        {
            var list = new List<VueTreeSelectModel>();
            var nodes = allNodes.Where(x => x.pId == node.id).ToList();

            if (nodes.Count > 0)
            {
                nodes.ForEach((x) =>
                {
                    var nodeChildren = GetTreeSelectModels(allNodes, x);
                    list.Add(new VueTreeSelectModel() { id = x.id, label = x.name, children = nodeChildren?.ToArray() });
                });
            }
            else
            {
                return null;
            }
            return list;
        }

    }
}
