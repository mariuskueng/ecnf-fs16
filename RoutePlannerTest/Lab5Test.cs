using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    public class Lab05Test
    {
        [TestMethod]
        public void TestFindCitiesByTransportMode()
        {
            Cities cities = new Cities();
            cities.ReadCities(@"citiesTestDataLab3.txt");
            var routes = new Routes(cities);
            routes.ReadRoutes(@"linksTestDataLab3.txt");

            City[] citiesByMode = routes.FindCities(TransportMode.Rail);
            Assert.AreEqual(11, citiesByMode.Length);

            City[] emptyCitiesByMode = routes.FindCities(TransportMode.Bus);
            Assert.AreEqual(0, emptyCitiesByMode.Length);
        }

        [TestMethod]
        public void TestFindCitiesByTransportModeIsASingleLINQStatement()
        {
            Func<TransportMode,City[]> method = new Routes(null).FindCities;

            Assert.IsTrue(method.Method.GetMethodBody().LocalVariables.Count<=2, "Implement the method FindCities as a single-line LINQ statement in the form \"return [LINQ];\".");
        }
    }
}