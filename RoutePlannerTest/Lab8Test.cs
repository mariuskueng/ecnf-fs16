using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public class Lab8Test
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";

        private const string LinksTestFile = "linksTestDataLab4.txt";

        [TestMethod]
        public void TestLoadDynamicValid()
        {
            var cities = new Cities();
            // just test for correct dynamic creation of valid routed class from config
            IRoutes routes = RoutesFactory.Create(cities);
            Assert.IsInstanceOfType(routes, typeof(IRoutes));
            
            // now test for correct dynamic creation of valid routed class passed as string
            routes = RoutesFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Routes");
            Assert.IsInstanceOfType(routes, typeof(IRoutes));
        }
        
        [TestMethod]
        public void TestLoadDynamicInvalid()
        {
            var cities = new Cities();
            // pass a name of a class that does not exist
            try
            {
                IRoutes routes = RoutesFactory.Create(cities, "Class.Does.Not.Exist");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch(NotSupportedException)
            {
            }

            // pass a name of a class that exists, but does not implement the interface
            try
            {
                IRoutes routes = RoutesFactory.Create(cities, "Cities");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }


        [TestMethod]
        public void TestLoadAndRunDynamicSecondImpl()
        {
            var defaultSettings = Assembly.GetAssembly(typeof(IRoutes))
                .GetType("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Properties.Settings")
                .GetProperty("Default")
                .GetValue(null);
            defaultSettings.GetType().GetMethod("Reset").Invoke(defaultSettings, new object[] { });
            //defaultSettings.GetType().GetMethod("Reload").Invoke(defaultSettings, new object[] { });

            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            IRoutes routes = RoutesFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.TestRoutesValidConstructor");
            Assert.AreEqual(42, routes.ReadRoutes("nonsense"));

            IRoutes routes2 = RoutesFactory.Create(cities);
            Assert.AreNotEqual(routes.GetType(), routes2.GetType());

            try
            {
                try
                {
                    defaultSettings.GetType().GetProperty("RouteAlgorithm").SetValue(defaultSettings, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.TestRoutesValidConstructor");
                }
                catch
                {
                    Assert.Fail("Make sure there is a String-property called \"RouteAlgorithm\" with User-scope.");
                }

                IRoutes routes3 = RoutesFactory.Create(cities);
                Assert.AreEqual(routes.GetType(), routes3.GetType());
            }
            finally
            {
                defaultSettings.GetType().GetMethod("Reset").Invoke(defaultSettings, new object[] { });
            }

            try
            {
                RoutesFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab5Test.TestRoutesInvalidConstructor");
                Assert.Fail("Should throw a NotSupportedException, because it doesn't have the right constructor");
            }
            catch(NotSupportedException)
            {
            }

            try
            {
                RoutesFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab5Test.TestRoutesNoInterface");
                Assert.Fail("Should throw a NotSupportedException, because IRoutes is not implemented");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestLoadAndRunDynamicClassicImpl()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            IRoutes routes = RoutesFactory.Create(cities);

            routes.ReadRoutes(LinksTestFile);

            Assert.AreEqual(28, cities.Count);

            // test available cities
            List<Link> links = routes.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);

            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                                       new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Assert.IsNotNull(links);
            Assert.AreEqual(expectedLinks.Count, links.Count);

            for (int i = 0; i < links.Count; i++)
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == links[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == links[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == links[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == links[i].FromCity.Name));

            try
            {
                links = routes.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw a KeyNotFoundException");
            }
            catch(KeyNotFoundException)
            {
            }
        }
    }

    class TestRoutesInvalidConstructor : IRoutes
    {
        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadRoutes(string filename)
        {
            return 42;
        }
    }

    class TestRoutesValidConstructor : IRoutes
    {
        public TestRoutesValidConstructor(Cities _c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadRoutes(string filename)
        {
            return 42;
        }
    }

    class TestRoutesNoInterface
    {
        public TestRoutesNoInterface(Cities _c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadRoutes(string filename)
        {
            return 42;
        }
    }
}

