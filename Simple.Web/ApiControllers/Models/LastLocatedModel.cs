using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.ApiControllers.Models
{

    public class LastLocatedModel
    {
        
        public string Name { get; set; }
        
        public string Mac { get; set; }
        
        public double Longitude { get; set; }
        
        public double Latitude { get; set; }
        
        public DateTime Gnsstime { get; set; }
        
        public int Heading { get; set; }

        public int Speed { get; set; }

    }
}
