using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.IO;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System;
using System.Threading;
using System.Globalization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    public class Lab8DeSerializerTest
    {
        [TestMethod]
        public void TestSerializeSingleCityWithValues()
        {
            var c = new City("Aarau", "Switzerland", 10, 1.1, 2.2);

            TextWriter stream = new StringWriter();
            var writer = new SimpleObjectWriter(stream);
            writer.Next(c);
            var result = stream.ToString();

            Assert.AreEqual(CityWithValues, result);
        }

        private const string CityWithValues =
              "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\n"
            + "Country=\"Switzerland\"\r\n"
            + "Location is a nested object...\r\n"
            + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\n"
            + "Latitude=1.1\r\n"
            + "Longitude=2.2\r\n"
            + "Name=\"Aarau\"\r\n"
            + "End of instance\r\n"
            + "Name=\"Aarau\"\r\n"
            + "Population=10\r\n"
            + "End of instance\r\n";

        [TestMethod]
        public void TestSerializeMultCitiesWithValues()
        {
            const string expectedString1 =
                  "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\n"
                + "Country=\"Switzerland\"\r\n"
                + "Location is a nested object...\r\n"
                + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\n"
                + "Latitude=1.1\r\n"
                + "Longitude=2.2\r\n"
                + "Name=\"Aarau\"\r\n"
                + "End of instance\r\n"
                + "Name=\"Aarau\"\r\n"
                + "Population=10\r\n"
                + "End of instance\r\n";

            const string expectedString2 =
                  "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\n"
                + "Country=\"Switzerland\"\r\n"
                + "Location is a nested object...\r\n"
                + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\n"
                + "Latitude=1.1\r\n"
                + "Longitude=2.2\r\n"
                + "Name=\"Bern\"\r\n"
                + "End of instance\r\n"
                + "Name=\"Bern\"\r\n"
                + "Population=10\r\n"
                + "End of instance\r\n";
            const string expectedString = expectedString1 + expectedString2;
            var c1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var c2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);

            var stream = new StringWriter();
            var writer = new SimpleObjectWriter(stream);
            writer.Next(c1);
            var result = stream.ToString();
            Assert.AreEqual(expectedString1, result);

            // write second city
            writer.Next(c2);

            // result is expected to contain both cities
            result = stream.ToString();
            Assert.AreEqual(expectedString, result);
        }



        [TestMethod]
        public void TestDeserializeSingleCityWithValues()
        {
            var expectedCity = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            TextReader stream = new StringReader(CityWithValues);
            var reader = new SimpleObjectReader(stream);
            var city = reader.Next() as City;
            Assert.IsNotNull(city);
            Assert.AreEqual(expectedCity.Name, city.Name);
            Assert.AreEqual(expectedCity.Country, city.Country);
            Assert.AreEqual(expectedCity.Location.Latitude, city.Location.Latitude);
        }

        [TestMethod]
        public void TestDeserializeMultCitiesWithValues()
        {
            const string cityString1 =
                  "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\n"
                + "Country=\"Switzerland\"\r\n"
                + "Location is a nested object...\r\n"
                + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\n"
                + "Latitude=1.1\r\n"
                + "Longitude=2.2\r\n"
                + "Name=\"Aarau\"\r\n"
                + "End of instance\r\n"
                + "Name=\"Aarau\"\r\n"
                + "Population=10\r\n"
                + "End of instance\r\n";
            const string cityString2 =
                  "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\n"
                + "Country=\"Switzerland\"\r\n"
                + "Location is a nested object...\r\n"
                + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\n"
                + "Latitude=1.1\r\n"
                + "Longitude=2.2\r\n"
                + "Name=\"Bern\"\r\n"
                + "End of instance\r\n"
                + "Name=\"Bern\"\r\n"
                + "Population=10\r\n"
                + "End of instance\r\n";
            const string cityString = cityString1 + cityString2;
            var expectedCity1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var expectedCity2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);
            var reader = new SimpleObjectReader(new StringReader(cityString));
            var city1 = reader.Next() as City;

            Assert.IsNotNull(city1);
            Assert.AreEqual(expectedCity1.Name, city1.Name);
            Assert.AreEqual(expectedCity1.Country, city1.Country);
            Assert.AreEqual(expectedCity1.Location.Latitude, city1.Location.Latitude);
            var city2 = reader.Next() as City;
            Assert.IsNotNull(city2);
            Assert.AreEqual(expectedCity2.Name, city2.Name);
            Assert.AreEqual(expectedCity2.Country, city2.Country);
            Assert.AreEqual(expectedCity2.Location.Latitude, city2.Location.Latitude);
        }

        [TestMethod]
        public void TestSerializeOtherThings()
        {
            const string expected =
                  "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.SerializeTest\r\n"
                + "ADouble=0.54444\r\n"
                + "BInt=1\r\n"
                + "CString=\"a\"\r\n"
                + "End of instance\r\n";
            var obj = new SimpleObjectReader(new StringReader(expected)).Next() as SerializeTest;

            Assert.AreEqual(obj.CString, "a");
            Assert.AreEqual(obj.ADouble, 0.54444);
            Assert.AreEqual(obj.BInt, 1);

            {
                var sw = new StringWriter();
                new SimpleObjectWriter(sw).Next(obj);
                Assert.AreEqual(expected, sw.ToString());
            }

            {
                var tl = new ThirdLevel()
                {
                    t = new SecondLevel()
                    {
                        t = obj
                    }
                };

                var sw = new StringWriter();
                new SimpleObjectWriter(sw).Next(tl);
                Assert.AreEqual(
                      "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.ThirdLevel\r\n"
                    + "t is a nested object...\r\n"
                    + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.SecondLevel\r\n"
                    + "t is a nested object...\r\n"
                    + "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.SerializeTest\r\n"
                    + "ADouble=0.54444\r\n"
                    + "BInt=1\r\n"
                    + "CString=\"a\"\r\n"
                    + "End of instance\r\n"
                    + "End of instance\r\n"
                    + "End of instance\r\n", sw.ToString());
            }
        }

        [TestMethod]
        public void TestSerializationCulture()
        {
            const string expected =
                    "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.SerializeTest\r\n"
                + "ADouble=0.54444\r\n"
                + "BInt=1\r\n"
                + "CString=\"a\"\r\n"
                + "End of instance\r\n";

            var previousCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                // should work, even in "weird" cultures,
                // see http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-CH");
                var obj1 = new SimpleObjectReader(new StringReader(expected)).Next() as SerializeTest;
                var sw1 = new StringWriter();
                new SimpleObjectWriter(sw1).Next(obj1);

                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                var obj2 = new SimpleObjectReader(new StringReader(expected)).Next() as SerializeTest;
                var sw2 = new StringWriter();
                new SimpleObjectWriter(sw2).Next(obj2);

                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
                var obj3 = new SimpleObjectReader(new StringReader(expected)).Next() as SerializeTest;
                var sw3 = new StringWriter();
                new SimpleObjectWriter(sw3).Next(obj3);

                Assert.AreEqual(sw1.ToString(), sw2.ToString());
                Assert.AreEqual(sw2.ToString(), sw3.ToString());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previousCulture;
            }
        }
    }

    class ThirdLevel
    {
        public SecondLevel t { get; set; }
    }

    class SecondLevel
    {
        public SerializeTest t { get; set; }


    }

    class SerializeTest
    {
        public string CString { get; set; }
        public double ADouble { get; set; }
        public int BInt { get; set; }


        internal int ShouldNotBeSerialized1 { get; set; }
        private int ShouldNotBeSerialized2 { get; set; }
        public int ShouldNotBeSerialized3 = 0;


        public SerializeTest()
        {

        }
    }
}
