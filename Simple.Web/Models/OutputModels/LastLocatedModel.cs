using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models.OutputModels
{

    public class LastLocatedModel
    {
        
        public string License { get; set; }
        
        public string Mac { get; set; }
        
        public double Longitude { get; set; }
        
        public double Latitude { get; set; }
        
        public DateTime Gnsstime { get; set; }
        
        public int Heading { get; set; }

        public int Speed { get; set; }

    }
}
