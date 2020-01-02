using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.ZTree
{
    public class TreeNode
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pId { get; set; }
        public bool @checked { get; set; }
        public string icon { get; set; }
        public string iconClose { get; set; }
        public string iconOpen { get; set; }
        public string iconSkin { get; set; }
        public bool isParent { get; set; }
        public bool open { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TreeNode[] children { get; set; }
    }
}
