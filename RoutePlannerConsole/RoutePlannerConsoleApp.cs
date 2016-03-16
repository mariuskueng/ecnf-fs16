using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Routeplanner (Version {0})", Assembly.GetExecutingAssembly().GetName().Version);

            var windisch = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            var bern = new WayPoint("Bern", 46.9479739, 7.4474468);
            var tripolis = new WayPoint("Tripolis", 32.8872094, 13.1913383);
            var rio = new WayPoint("Rio de Janeiro", -22.908970, -43.175922);

            Console.WriteLine(windisch);
            Console.WriteLine(bern.Distance(tripolis));

            var cities = new Cities();
            cities.ReadCities("data/citiesTestDataLab4.txt");

            var routes = new Routes(cities);
            var count = routes.ReadRoutes("data/linksTestDataLab4.txt");
            var links = routes.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);

            foreach (var link in links)
            {
                Console.WriteLine($"{link.FromCity.Name} - {link.ToCity.Name}");
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
        
    }
}
