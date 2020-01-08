using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Areas.EzMap.Models
{
    public class QueryHistoryBackModel
    {
        public string DeviceId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MinSpeed { get; set; }

        public bool ZeroSpeed { get; set; }

    }
}
