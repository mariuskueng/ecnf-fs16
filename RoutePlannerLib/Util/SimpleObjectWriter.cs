using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        private TextWriter stream;
        public SimpleObjectWriter(TextWriter stream)
        {
            this.stream = stream;
        }

        public void Next(object obj)
        {
            var type = obj.GetType();
            stream.WriteLine("Instance of {0}", type.FullName);
            var properties = type.GetProperties();
            Array.Sort(properties,
                   delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                   {
                       return propertyInfo1.Name.CompareTo(propertyInfo2.Name);
                   });

            foreach (var property in properties)
            {
                if (!property.GetCustomAttributes(false).Any(attr => attr is XmlIgnoreAttribute))
                {
                    if (property.GetValue(obj) as string == null && property.GetValue(obj) as ValueType == null)
                    {
                        stream.WriteLine("{0} is a nested object...", property.Name);
                        Next(property.GetValue(obj));
                    }
                    else
                    {
                        if (property.GetValue(obj) is double)
                            stream.WriteLine("{0}={1}", property.Name, ((double)property.GetValue(obj)).ToString(CultureInfo.InvariantCulture));
                        else if (property.GetValue(obj) is string)
                            stream.WriteLine("{0}=\"{1}\"", property.Name, property.GetValue(obj).ToString());
                        else
                            stream.WriteLine("{0}={1}", property.Name, property.GetValue(obj).ToString());
                    }
                }
            }
            stream.WriteLine("End of instance");
        }
    }
}
