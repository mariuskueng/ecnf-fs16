using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        public List<City> cities;

        public Cities()
        {
            this.cities = new List<City>();
        }

        public int ReadCities(string filename)
        {
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
                                Int32.Parse(city[2]),
                                new WayPoint(city[0], Double.Parse(city[3]), Double.Parse(city[4])
                           )
                       ));
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    throw e;
                }


            }
            return this.cities.Count;   
        }
    }
}
