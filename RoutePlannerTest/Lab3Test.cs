using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    public class Lab3Test
    {
        private const string CitiesTestFile = "citiesTestDataLab3.txt";
        private const string LinksTestFile = "linksTestDataLab3.txt";

        [TestMethod]
        public void TestLinkTransportMode()
        {
            var mumbai = new City("Mumbai", "India", 12383146, 18.96, 72.82);
            var buenosAires = new City("Buenos Aires", "Argentina", 12116379, -34.61, -58.37);
            
            var link = new Link(mumbai, buenosAires, 10);
            
            // default transport should be Car
            Assert.AreEqual(TransportMode.Car, link.TransportMode);
            
            link = new Link(mumbai, buenosAires, 10, TransportMode.Ship);
            Assert.AreEqual(TransportMode.Ship, link.TransportMode);
        }

        [TestMethod]
        public void TestTask1FindCityInCities()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            
            try
            {
                var x=cities["noCity"];
                Assert.Fail("Indexer Cities[string] should throw a KeyNotFoundException when the supplied City cannot be found.");
            }
            catch (KeyNotFoundException)
            {
            }
            
            Assert.AreEqual("Zürich", cities["Zürich"].Name);
            
            // should be case insensitive
            Assert.AreEqual("Zürich", cities["züRicH"].Name);
            
            // should be case insensitive, even in "weird" cultures,
            // see http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Assert.AreEqual("Zürich", cities["züRicH"].Name);
            Assert.AreEqual("Zürich", cities["züRIcH"].Name);
            Thread.CurrentThread.CurrentCulture = previousCulture;
            
            // should be picky about spaces
            try
            {
                var x=cities["züRicH "];
                Assert.Fail("Indexer Cities[string] should be picky about leading/trailing spaces.");
            }
            catch (KeyNotFoundException)
            {
            }
        }
        
        [TestMethod]
        public void TestRoutesReadLinks()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            
            Assert.AreEqual(7, new Routes(cities).ReadRoutes(LinksTestFile));
        }
        
        [TestMethod]
        public void TestTask2FiredEvents()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            
            var routes = new Routes(cities);

            // test available cities
            routes.RouteRequested += (sender, e) =>
            {
                Assert.AreEqual("Bern", e.FromCity.Name);
                Assert.AreEqual("Zürich", e.ToCity.Name);
            };
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportMode.Rail);

            // test case insensitivity
            routes.FindShortestRouteBetween("BeRN", "ZüRiCh", TransportMode.Rail);

            // test not existing cities
            routes = new Routes(cities);
            routes.RouteRequested += (sender, e) =>
            {
                Assert.Fail("Listeners should only be informed about valid requests.");
            };
            try
            {
                routes.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw KeyNotFoundException when called with invalid city names.");
            }
            catch(KeyNotFoundException)
            {
            }
        }
        
        [TestMethod]
        public void TestTask2EventWithNoObserver()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            
            var routes = new Routes(cities);
            
            // should run without exception
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportMode.Rail);
        }
        
        [TestMethod]
        public void TestRequestWatcher()
        {
            var reqWatch = new RouteRequestWatcher();
            
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            
            var routes = new Routes(cities);
            
            routes.RouteRequested += reqWatch.LogRouteRequests;
            
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportMode.Rail);
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportMode.Rail);
            routes.FindShortestRouteBetween("Basel", "Bern", TransportMode.Rail);
            
            Assert.AreEqual(reqWatch.GetCityRequests(cities["Zürich"]), 2);
            Assert.AreEqual(reqWatch.GetCityRequests(cities["Bern"]), 1);
            Assert.AreEqual(reqWatch.GetCityRequests(cities["Basel"]), 0);
            Assert.AreEqual(reqWatch.GetCityRequests(cities["Lausanne"]), 0);
        }
    }
}
