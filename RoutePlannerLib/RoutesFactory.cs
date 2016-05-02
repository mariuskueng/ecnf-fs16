using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoutePlannerLib.Properties;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RoutesFactory
    {
        static public IRoutes Create(Cities cities)
        {
            var routeAlgorithm = Settings.Default.Properties["RouteAlgorithm"].ToString();
            return Create(cities, routeAlgorithm);
        }
        static public IRoutes Create(Cities cities, string algorithmClassName)
        {
            IRoutes routeFactory = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type t = null;
            foreach (var assembly in assemblies)
            {
                t = assembly.GetType(algorithmClassName);
                if (t != null)
                {
                    routeFactory = (IRoutes)Activator.CreateInstance(t, cities);
                    return routeFactory as IRoutes;
                }
            }

            throw new NotSupportedException(algorithmClassName + " is not supported.");
        }
    }
}
