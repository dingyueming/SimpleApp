using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.VueTreeSelect
{
    public class VueTreeSelectModel
    {
        public string id { get; set; }

        public string label { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public VueTreeSelectModel[] children { get; set; }

        public string Tag { get; set; }
    }
}
