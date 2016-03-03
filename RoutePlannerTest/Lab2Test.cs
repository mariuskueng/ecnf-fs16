using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Globalization;
using System.IO;
using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// This test class contains all tests for Lab 2
    /// 
    /// REMARK:
    /// Putting all tests of a lab is just for convenience; 
    /// usually make, at least, one test file per "class under test" (CUT).
    ///</summary>
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    public class Lab2Test
    {
        private const string citiesTestFile = "citiesTestDataLab2.txt";

        /// <summary>
        /// A test for WayPoint Constructor        
        /// </summary>
        [TestMethod]
        public void TestWayPointValidConstructor()
        {
            var target = new WayPoint("Windisch", 0.564, 0.646);

            Assert.AreEqual("Windisch", target.Name);
            Assert.AreEqual(0.564, target.Latitude);
            Assert.AreEqual(0.646, target.Longitude);
        }

        [TestMethod]
        public void TestWayPointToString()
        {
            // test complete way point
            var target = new WayPoint("Windisch", 0.564, 0.646);
            Assert.AreEqual(target.ToString(), "WayPoint: Windisch 0.56/0.65");

            // test no-name case
            target = new WayPoint(null, 0.564, 0.646);
            Assert.AreEqual(target.ToString(), "WayPoint: 0.56/0.65");

            // test for correct formatting with 2 decimal places
            var targetRound = new WayPoint("Testtest", 1.0, 0.50);
            Assert.AreEqual(targetRound.ToString(), "WayPoint: Testtest 1.00/0.50");
        }

        [TestMethod]
        public void TestCultureHandling()
        {
            var target = new WayPoint("Windisch", 0.564, 0.646);

            // test whether formatting works correctly in all cultures
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            var newCulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Assert.AreEqual("WayPoint: Windisch 0.56/0.65", target.ToString());
            Assert.AreEqual(Thread.CurrentThread.CurrentCulture, newCulture);

            newCulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("de-CH");
            Assert.AreEqual("WayPoint: Windisch 0.56/0.65", target.ToString());
            Assert.AreEqual(Thread.CurrentThread.CurrentCulture, newCulture);

            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [TestMethod]
        public void TestWayPointDistanceCalculation()
        {
            var bern = new WayPoint("Bern", 46.95, 7.44);
            var tripolis = new WayPoint("Tripolis", 32.876174, 13.187507);
            var actual = bern.Distance(tripolis);
            Assert.IsFalse(double.IsNaN(actual));
            Assert.AreEqual(1638.74410788167, actual, 0.001);

            actual = tripolis.Distance(bern);
            Assert.IsFalse(double.IsNaN(actual));
            Assert.AreEqual(1638.74410788167, actual, 0.001);
        }

        /// <summary>
        ///A test for City Constructor        
        /// </summary>
        [TestMethod]
        public void TestCityValidConstructor()
        {
            const double latitude = 47.479319847061966;
            const double longitude = 8.212966918945312;
            const int population = 75000;
            const string name = "Bern";
            const string country = "Schweiz";

            var bern = new City(name, country, population, latitude, longitude);

            Assert.AreEqual(name, bern.Name);
            Assert.AreEqual(name, bern.Location.Name); // city name == wayPoint name
            Assert.AreEqual(population, bern.Population);
            Assert.AreEqual(longitude, bern.Location.Longitude, 0.001);
            Assert.AreEqual(latitude, bern.Location.Latitude, 0.001);
        }

        [TestMethod]
        public void TestFileClose()
        {
            //create a new empty file
            var tempFN=Path.GetTempFileName();
            using (var fs = new StreamWriter(tempFN))
            {
            }

            Assert.AreEqual(0, new Cities().ReadCities(tempFN));

            //see whether someone is still using the file --> shouldn't throw an exception
            File.Delete(tempFN);
        }

        [TestMethod]
        public void TestReadCitiesFileMissing()
        {
            var cities = new Cities();

            try
            {
                cities.ReadCities("doesnotexist");
                Assert.Fail();
            }
            catch(FileNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestReadCitiesAndIndexer()
        {
            const int expectedCities = 10;
            var cities = new Cities();

            Assert.AreEqual(expectedCities, cities.ReadCities(citiesTestFile));

            Assert.AreEqual(expectedCities, cities.Count);

            // read cities once again; cities should be added to the list
            Assert.AreEqual(expectedCities, cities.ReadCities(citiesTestFile));

            // total count should be doubled
            Assert.AreEqual(2 * expectedCities, cities.Count);

            //verify first and last city
            Assert.AreEqual("Mumbai", cities[0].Name);
            Assert.AreEqual("Jakarta", cities[9].Name);

            // check for invalid index
            try
            {
                var c=cities[-1];
                Assert.Fail("Invalid index not handled properly");
            }
            catch(IndexOutOfRangeException _iore)
            {
                Assert.IsTrue(_iore.Message.Length>2, "IndexOutOfRangeException has no meaningful description");
            }
            catch
            {
                Assert.Fail("Wrong exception type thrown on invalid index");
            }

            try
            {
                var c = cities[100];
                Assert.Fail("Invalid index not handled properly");
            }
            catch (IndexOutOfRangeException _iore)
            {
                Assert.IsTrue(_iore.Message.Length > 2, "IndexOutOfRangeException has no meaningful description");
            }
            catch
            {
                Assert.Fail("Wrong exception type thrown on invalid index");
            }

            // test in other cultures
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Assert.AreEqual(expectedCities, new Cities().ReadCities(citiesTestFile));

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-CH");
            Assert.AreEqual(expectedCities, new Cities().ReadCities(citiesTestFile));
            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [TestMethod]
        public void TestFindNeighbours()
        {
            var cities = new Cities();
            cities.ReadCities(citiesTestFile);

            var loc = cities[0].Location;

            IEnumerable<City> neighbors = cities.FindNeighbours(loc, 2000);

            //verifies if the correct cities were found
            Assert.AreEqual(4, neighbors.Count());
            Assert.IsTrue(neighbors.Any(c => c.Name == "Mumbai"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Karachi"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Dhaka"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Dilli"));
        }

        [Ignore]
        [TestMethod]
        public void TestFindNeighboursSorted()
        {
            var cities = new Cities();
            cities.ReadCities(citiesTestFile);

            var loc = cities[0].Location;

            var neighbors = cities.FindNeighbours(loc, 2000).ToArray();

            //verify the correct order (sorted  by distance)
            Assert.AreEqual(4, neighbors.Length);
            Assert.AreEqual("Mumbai", neighbors[0].Name);
            Assert.AreEqual("Karachi", neighbors[1].Name);
            Assert.IsTrue(loc.Distance(neighbors[0].Location) <= loc.Distance(neighbors[1].Location));
            Assert.AreEqual("Dilli", neighbors[2].Name);
            Assert.IsTrue(loc.Distance(neighbors[1].Location) <= loc.Distance(neighbors[2].Location));
            Assert.AreEqual("Dhaka", neighbors[3].Name);
            Assert.IsTrue(loc.Distance(neighbors[2].Location) <= loc.Distance(neighbors[3].Location));
        }
    }
}
