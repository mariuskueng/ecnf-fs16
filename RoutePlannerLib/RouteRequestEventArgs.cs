using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : System.EventArgs
    {
        public City FromCity { get; set; }
        public City ToCity { get; set; }

        public TransportMode transportMode;

        public RouteRequestEventArgs(City from, City to, TransportMode trans)
        {
            this.FromCity = from;
            this.ToCity = to;
            this.transportMode = trans;
        }
    }
}