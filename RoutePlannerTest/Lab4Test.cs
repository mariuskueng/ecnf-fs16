using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public class Lab4Test
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";
        private const string LinksTestFile = "linksTestDataLab4.txt";

        [TestMethod]
        public void TestWayPointCalculation()
        {
            var wp1 = new WayPoint("Home", 10.4, 20.8);
            var wp2 = new WayPoint("Target", 1.2, 2.4);

            var addWp = wp1 + wp2;
            if (object.ReferenceEquals(addWp, wp1) || object.ReferenceEquals(addWp, wp2))
                Assert.Fail("Operations must put results in *new* objects");

            Assert.AreEqual(wp1.Name, addWp.Name);
            Assert.AreEqual(wp1.Latitude + wp2.Latitude, addWp.Latitude);
            Assert.AreEqual(wp1.Longitude + wp2.Longitude, addWp.Longitude);

            var minWp = wp1 - wp2;
            if (object.ReferenceEquals(minWp, wp1) || object.ReferenceEquals(minWp, wp2))
                Assert.Fail("Operations must put results in *new* objects");

            Assert.AreEqual(minWp.Name, wp1.Name);
            Assert.AreEqual(minWp.Latitude, wp1.Latitude - wp2.Latitude);
            Assert.AreEqual(minWp.Longitude, wp1.Longitude - wp2.Longitude);

            var minWp2 = wp2 - wp1;
            Assert.AreEqual(minWp2.Name, wp2.Name);
            Assert.AreEqual(minWp2.Latitude, wp2.Latitude - wp1.Latitude);
            Assert.AreEqual(minWp2.Longitude, wp2.Longitude - wp1.Longitude);
        }

        [TestMethod]
        public void TestTask4ReadRoutes()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var links = new Routes(cities);
            var count = links.ReadRoutes(LinksTestFile);
            Assert.AreEqual(30, count);
            Assert.AreEqual(30, links.Count);
        }


        [TestMethod]
        public void TestTask4FindRoutes()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                new City("Basel", "Switzerland", 7000, 1, 2), 0));

            var links = new Routes(cities);
            links.ReadRoutes(LinksTestFile);

            Assert.AreEqual(28, cities.Count);

            // test available cities
            var route = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail); 
            Assert.IsNotNull(route);
            Assert.AreEqual(route.Count, expectedLinks.Count);

            for (var i = 0; i < route.Count; i++)
            {
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == route[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == route[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == route[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == route[i].FromCity.Name));
            }

            // test some other route
            route = links.FindShortestRouteBetween("Zürich", "Milano", TransportMode.Rail);
            Assert.AreEqual(5, route.Count);

            // test when no routes can be found
            route = links.FindShortestRouteBetween("Zürich", "Le Havre", TransportMode.Rail);
            Assert.IsNull(route);

            try
            {
                route = links.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw a KeyNotFoundException");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void TestGetSplittedLines()
        {

            using (TextReader tr = new StreamReader(CitiesTestFile))
            {
                var citiesAsStrings = tr.GetSplittedLines('\t').ToArray();

                // just check the name of the first and last city in list
                Assert.AreEqual("Zürich", citiesAsStrings[0][0].Trim()); // name
                Assert.AreEqual("Altdorf", citiesAsStrings[citiesAsStrings.Length - 1][0].Trim()); // name


            }
        }
    }
}
