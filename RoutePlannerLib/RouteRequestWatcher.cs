using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        public Dictionary<City, int> requests;
        
        public RouteRequestWatcher()
        {
            requests = new Dictionary<City, int>();
        }

        public void LogRouteRequests(object sender, RouteRequestEventArgs args)
        {
            Console.WriteLine("Current Request State");
            Console.WriteLine("---------------------");

            foreach (var r in requests)
            {
                Console.WriteLine("ToCity: ${r.Key.Name} has been requested ${r.Value} times");
            }
        }

        public int GetCityRequests(City city)
        {
            return requests.Count;
        }
    }
}