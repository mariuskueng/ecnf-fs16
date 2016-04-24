using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities cities;
        public World(Cities c)
        {
            cities = c;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args,
                out object result)
        {
            try
            {
                City city = cities[binder.Name];
                result = city;
            }
            catch (KeyNotFoundException)
            {

                result = $"The city \"{binder.Name}\" does not exist!";
            }

            return true;
        }
    }
}
