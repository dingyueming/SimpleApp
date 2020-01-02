using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.BootStrapTreeView
{
    public class TreeNode
    {
        /// <summary>
        /// 列表树节点上的文本，通常是节点右边的小图标
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 列表树节点上的图标，通常是节点左边的图标
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 当某个节点被选择后显示的图标，通常是节点左边的图标
        /// </summary>
        public string selectedIcon { get; set; }
        /// <summary>
        /// 结合全局enableLinks选项为列表树节点指定URL。
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 指定列表树的节点是否可选择。设置为false将使节点展开，并且不能被选择。
        /// </summary>
        public bool selectable { get; set; } = true;
        /// <summary>
        /// 一个节点的初始状态。
        /// </summary>
        public State state { get; set; }
        /// <summary>
        /// 节点的前景色，覆盖全局的前景色选项。
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 节点的背景色，覆盖全局的背景色选项。
        /// </summary>
        public string backColor { get; set; }
        /// <summary>
        /// 通过结合全局showTags选项来在列表树节点的右边添加额外的信息
        /// </summary>
        public string[] tags { get; set; }
        /// <summary>
        /// data
        /// </summary>
        public TreeNode[] nodes { get; set; }
    }
    public class State
    {
        /// <summary>
        /// 指示一个节点是否处于checked状态，用一个checkbox图标表示。
        /// </summary>
        public bool @checked { get; set; }
        /// <summary>
        /// 指示一个节点是否处于disabled状态。（不是selectable，expandable或checkable）
        /// </summary>
        public bool @disabled { get; set; }
        /// <summary>
        /// 指示一个节点是否处于展开状态。
        /// </summary>
        public bool @expanded { get; set; }
        /// <summary>
        /// 指示一个节点是否可以被选择。
        /// </summary>
        public bool @selected { get; set; }
    }
}
