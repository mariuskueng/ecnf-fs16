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

                    try
                    {
                        var city1 = cities[tokens[0]];
                        var city2 = cities[tokens[1]];
                        routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
                    }
                    catch (Exception)
                    {

                        // This exception is unhandled by TeskTask4ReadRoutes which tries
                        // to get routes of non-exisiting cities.
                    }

                }
            }
            
            return Count - previousCount;
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            if (RouteRequested != null)
            {
                RouteRequested(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));
            }

            //use dijkstra's algorithm to look for all single-source shortest paths
            var visited = new Dictionary<City, DijkstraNode>();
            var pending = new SortedSet<DijkstraNode>(new DijkstraNode[]
            {
                new DijkstraNode()
                {
                    VisitingCity = cities[fromCity],
                    Distance = 0
                }
            });

            while (pending.Any())
            {
                var cur = pending.Last();
                pending.Remove(cur);

                if (!visited.ContainsKey(cur.VisitingCity))
                {
                    visited[cur.VisitingCity] = cur;

                    foreach (var link in GetListOfAllOutgoingRoutes(cur.VisitingCity, mode))
                        pending.Add(new DijkstraNode()
                        {
                            VisitingCity = (link.FromCity == cur.VisitingCity) ? link.ToCity : link.FromCity,
                            Distance = cur.Distance + link.Distance,
                            PreviousCity = cur.VisitingCity
                        });
                }
            }

            //did we find any route?
            if (!visited.ContainsKey(cities[toCity]))
                return null;

            //create a list of cities that we passed along the way
            var citiesEnRoute = new List<City>();
            for (var c = cities[toCity]; c != null; c = visited[c].PreviousCity)
                citiesEnRoute.Add(c);
            citiesEnRoute.Reverse();

            //convert that city-list into a list of links
            IEnumerable<Link> paths = ConvertListOfCitiesToListOfLinks(citiesEnRoute);

            return paths.ToList();
        }

        private IEnumerable<Link> ConvertListOfCitiesToListOfLinks(List<City> citiesEnRoute)
        {
            var links = new List<Link>();
            for (int i = 0; i < citiesEnRoute.Count - 1; i++)
            {
                yield return new Link(citiesEnRoute[i], citiesEnRoute[i + 1], citiesEnRoute[i].Location.Distance(citiesEnRoute[i + 1].Location));
            }
        }


        private IEnumerable<Link> GetListOfAllOutgoingRoutes(City visitingCity, TransportMode mode)
        {
            foreach (var route in routes)
            {
                if (mode.Equals(route.TransportMode))
                {
                    if (visitingCity.Equals(route.FromCity) || visitingCity.Equals(route.ToCity))
                    {
                        yield return route;
                    }
                }
            }
        }

        private class DijkstraNode : IComparable<DijkstraNode>
        {
            public City VisitingCity;
            public double Distance;
            public City PreviousCity;

            public int CompareTo(DijkstraNode other)
            {
                return other.Distance.CompareTo(Distance);
            }
        }
    }
}
