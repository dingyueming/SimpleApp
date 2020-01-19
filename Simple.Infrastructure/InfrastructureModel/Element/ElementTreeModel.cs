using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.Element
{
    public class ElementTreeModel
    {
        public string id { get; set; }

        public string label { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ElementTreeModel[] children { get; set; }
    }
}
