using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.BootStrapTreeView
{
    public class TreeView
    {
        /// <summary>
        /// 列表树上显示的数据。
        /// </summary>
        public TreeNode[] data { get; set; }
        /// <summary>
        /// 设置所有列表树节点的背景颜色。
        /// </summary>
        public string backColor { get; set; }
        /// <summary>
        /// 设置列表树容器的边框颜色，如果不想要边框可以设置showBorder属性为false。
        /// </summary>
        public string borderColor { get; set; }
        /// <summary>
        /// 设置处于checked状态的复选框图标。
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-check"</example>
        public bool checkedIcon { get; set; }
        /// <summary>
        /// 设置列表树可收缩节点的图标。
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-minus"</example>
        public string collapseIcon { get; set; }
        /// <summary>
        /// 设置列表树所有节点的前景色。
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 设置列表树中没有子节点的节点的图标。
        /// </summary>
        public string emptyIcon { get;set;}
        /// <summary>
        /// 是否使用当前节点的文本作为超链接。超链接的href值必须在每个节点的data结构中给出。
        /// </summary>
        public bool enableLinks { get; set; } = false;
        /// <summary>
        /// 设置列表树可展开节点的图标。
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-plus"</example>
        public string expandIcon { get; set; }
        /// <summary>
        /// 是否高亮搜索结果。
        /// </summary>
        public bool highlightSearchResults { get; set; } = true;
        /// <summary>
        /// 当选择节点时是否高亮显示。
        /// </summary>
        public bool highlightSelected { get; set; } = true;
        /// <summary>
        /// 设置列表树的节点在用户鼠标滑过时的背景颜色。
        /// </summary>
        /// <example>	所有合法的颜色值， Default: '#F5F5F5'。</example>
        public string onhoverColor { get; set; }
        /// <summary>
        /// 设置继承树默认展开的级别
        /// </summary>
        public int levels { get; set; } = 2;
        /// <summary>
        /// 是否可以同时选择多个节点。
        /// </summary>
        public bool multiSelect { get; set; } = false;
        /// <summary>
        /// 设置所有列表树节点上的默认图标。
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-stop"</example>
        public string nodeIcon { get; set; }
        /// <summary>
        /// 设置所有被选择的节点上的默认图标。
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-stop"</example>
        public string selectedIcon { get; set; }
        /// <summary>
        /// 设置搜索结果节点的背景颜色。
        /// </summary>
        /// <example>所有合法的颜色值， Default: undefined, inherits。</example>
        public string searchResultBackColor { get; set; }
        /// <summary>
        /// 设置搜索结果节点的前景颜色。
        /// </summary>
        /// <example>	所有合法的颜色值， Default: '#D9534F'</example>
        public string searchResultColor { get; set; }
        /// <summary>
        /// 设置被选择节点的背景颜色。
        /// </summary>
        /// <example>所有合法的颜色值， Default: '#428bca'</example>
        public string selectedBackColor { get; set; }
        /// <summary>
        /// 设置列表树选择节点的背景颜色。。
        /// </summary>
        /// <example>所有合法的颜色值， Default: '#FFFFFF'。</example>
        public string selectedColor { get; set; }
        /// <summary>
        /// 是否在节点上显示边框。
        /// </summary>
        public bool showBorder { get; set; } = true;
        /// <summary>
        /// 是否在节点上显示checkboxes。。
        /// </summary>
        public bool showCheckbox { get; set; } = false;
        /// <summary>
        /// 是否显示节点图标。
        /// </summary>
        public bool showIcon { get; set; } = true;
        /// <summary>
        /// 是否在每个节点右边显示tags标签。tag值必须在每个列表树的data结构中给出。。。
        /// </summary>
        public bool showTags { get; set; } = false;
        /// <summary>
        /// 设置图标为未选择状态的checkbox图标
        /// </summary>
        /// <example>Bootstrap Glyphicons定义的 "glyphicon glyphicon-unchecked"</example>
        public string uncheckedIcon { get; set; }
    }
}
