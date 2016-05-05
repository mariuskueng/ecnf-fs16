using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        public List<City> cities;
        public int Count
        {
            get { return this.cities.Count; }
        }
        private static TraceSource traceSource =
            new TraceSource("Cities");

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
            traceSource.TraceData(TraceEventType.Information, 0, "ReadCities started");
            try
            {
                using (TextReader tr = new StreamReader(filename))
                {
                    IEnumerable<string[]> citiesAsStrings = tr.GetSplittedLines('\t');

                    var newCities = citiesAsStrings
                        .Select(cs => new City(
                            cs[0].Trim(), cs[1].Trim(),
                            int.Parse(cs[2]),
                            double.Parse(cs[3], CultureInfo.InvariantCulture),
                            double.Parse(cs[4], CultureInfo.InvariantCulture))
                        )
                        .ToList();

                    cities.AddRange(newCities);
                    traceSource.TraceData(TraceEventType.Information, 0, "ReadCities ended");
                    return newCities.Count;
                }
            }
            catch (FileNotFoundException e)
            {
                traceSource.TraceData(TraceEventType.Critical, 1, e.StackTrace);
                throw e;
            }
            
        }

        public IEnumerable<City> FindNeighbours(WayPoint location, double distance)
        {
            return cities
                .Where(c => c.Location.Distance(location) < distance)
                .OrderBy(o => location.Distance(o.Location));
   
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
