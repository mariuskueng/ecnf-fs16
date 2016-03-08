using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    ///	<summary>
    ///	Manages	a routes from a	city to	another	city.
    ///	</summary>
    ///	
    public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);

    public class Routes
    {
        private List<Link> routes = new List<Link>();
        private Cities cities;
        
        public event RouteRequestHandler RouteRequested;

        public int Count
        {
            get
            {
                return routes.Count;
            }
        }

        ///	<summary>
        ///	Initializes	the	Routes with	the	cities.
        ///	</summary>
        ///	<param name="cities"></param>
        public Routes(Cities _cities)
        {
            cities = _cities;
        }

        ///	<summary>
        ///	Reads a	list of	links from the given file.
        ///	Reads only links where the cities exist.
        ///	</summary>
        ///	<param name="filename">name	of links file</param>
        ///	<returns>number	of read	route</returns>
        public int ReadRoutes(string _filename)
        {
            var previousCount = Count;
            using (var reader = new StreamReader(_filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var tokens = line.Split('\t');
                    
                    var city1 = cities[tokens[0]];
                    var city2 = cities[tokens[1]];
                    
                    routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
                }
            }
            
            return Count - previousCount;
        }

        public List<Link> FindShortestRouteBetween(string _fromCity, string _toCity, TransportMode _mode)
        {
            if (RouteRequested != null)
            { 
                RouteRequested(this, new RouteRequestEventArgs(cities[_fromCity], cities[_toCity], _mode));
            }
            return new List<Link>();
        }
    }
}
