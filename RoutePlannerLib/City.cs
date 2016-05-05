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

        public City()
        {

        }

        public City(String Name, String Country, int Population, WayPoint Location)
        {
            this.Name = Name;
            this.Country = Country;
            this.Population = Population;
            this.Location = Location;
        }

        public City(String Name, String Country, int Population, double latitude, double longitude)
        {
            this.Name = Name;
            this.Country = Country;
            this.Population = Population;
            this.Location = new WayPoint(Name, latitude, longitude);

        }

        public override bool Equals(object o)
        {
            if (o != null && o is City)
            {
                var otherCity = (City)o;
                return Name.Equals(otherCity.Name, StringComparison.InvariantCultureIgnoreCase) && Country.Equals(otherCity.Country, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
