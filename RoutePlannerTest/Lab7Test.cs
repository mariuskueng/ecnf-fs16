using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export;
//using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic;
using System.IO;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    public class Lab07Test
    {
        [TestMethod]
        public void TestExcelExport()
        {
            var excelFileName = Directory.GetCurrentDirectory() + @"\ExportTest.xlsx";

            var bern = new City("Bern", "Switzerland", 5000, 46.95, 7.44);
            var zuerich = new City("Zürich", "Switzerland", 100000, 32.876174, 13.187507);
            var aarau = new City("Aarau", "Switzerland", 10000, 35.876174, 12.187507);
            var links = new Link[]
            {
                new Link(bern, aarau, 15, TransportMode.Ship),
                new Link(aarau, zuerich, 20, TransportMode.Ship)
            };

            var excel = new ExcelExchange();
            excel.WriteToFile(excelFileName, links);
            Assert.IsTrue(File.Exists(excelFileName), excelFileName);

            //should not show dialog boxes or fail, should silently overwrite the file
            excel.WriteToFile(excelFileName, links);
            Assert.IsTrue(File.Exists(excelFileName), excelFileName);
        }

        [TestMethod]
        public void TestDynamicWorld()
        {
            var cities = new Cities();

            cities.ReadCities("citiesTestDataLab2.txt");

            dynamic world = new World(cities);

            dynamic karachi = world.Karachi();
            Assert.AreEqual("Karachi", karachi.Name);
            Assert.AreEqual(cities["Karachi"], karachi);

            string notFound = world.Entenhausen();
            Assert.AreEqual("The city \"Entenhausen\" does not exist!", notFound);
        }
    }
}
