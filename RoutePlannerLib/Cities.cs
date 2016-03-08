using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Threading;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        public List<City> cities;
        public int Count
        {
            get { return this.cities.Count; }
        }

        public Cities()
        {
            this.cities = new List<City>();
        }

        public City this[int index] //indexer implementation
        {
            get
            {
                if (index > this.cities.Count || index < 0)
                {
                    throw new IndexOutOfRangeException("Invalid index in cities list!");
                }
                else
                {
                    return this.cities[index];
                }
            }
            set { this.cities[index] = value; }
        }

        public City this[string cityName]
        {
            get
            {
                var city = this.cities.Find(FindCityDelegate(cityName));
                if (city != null)
                {
                    return city;
                }            
                else
                {
                    throw new KeyNotFoundException($"City {cityName} not found.");
                }
            }
        }


        public int ReadCities(string filename)
        {
            var count = 0;

            using (var reader = new StreamReader(filename))
            {
                String lines = reader.ReadToEnd();
                var citiesAsStrings = lines.Split('\n');

                try
                {
                    foreach (var c in citiesAsStrings)
                    {
                        var city = c.Split('\t');
                        this.cities.Add(
                            new City(
                                city[0],
                                city[1],
                                Convert.ToInt32(city[2]),
                                new WayPoint(
                                    city[0], 
                                    double.Parse(city[3], CultureInfo.InvariantCulture),
                                    double.Parse(city[4], CultureInfo.InvariantCulture)
                                )
                            )
                        );
                        count++;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    
                }

                reader.Close();

            }
            return count;   
        }

        public IEnumerable<City> FindNeighbours(WayPoint location, double distance)
        {
            List<City> neighbours = new List<City>();
            foreach (var city in this.cities)
            {
                if (city.Location.Distance(location) < distance) {
                    neighbours.Add(city);
                }
            }
            return neighbours.OrderBy(o => location.Distance(o.Location));
   
        }

        public Predicate<City> FindCityDelegate(string name)
        {
            return delegate (City c)
            {
                return (c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            };
        }
    }
}
