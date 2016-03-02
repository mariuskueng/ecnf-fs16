using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public String Name { get; set; }
        public String Country { get; set; }
        public int Population { get; set; }
        public WayPoint Location { get; set; }

        public City(String Name, String Country, int Population, WayPoint Location)
        {
            this.Name = Name;
            this.Country = Country;
            this.Population = Population;
            this.Location = Location;

        }
    }
}
