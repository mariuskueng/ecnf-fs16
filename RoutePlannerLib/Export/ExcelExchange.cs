//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Office.Interop.Excel;

//namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
//{
//    public class ExcelExchange
//    {
//        // http://csharp.net-informations.com/excel/csharp-create-excel.htm
//        Application excelApp;

//        public void WriteToFile(string fileName, IEnumerable<Link> links)
//        {
//            excelApp = new Application();
//            if (excelApp == null)
//            {
//                Console.WriteLine("Excel could not start!");
//                return;
//            }

//            Workbook workBook = excelApp.Workbooks.Add();
//            Worksheet workSheet = workBook.ActiveSheet;

//            Range formatRange = workSheet.get_Range("A1", "D1");
//            formatRange.EntireColumn.ColumnWidth = 25;
//            formatRange.Font.Size = 14;
//            formatRange.Font.Bold = true;
//            formatRange.Cells.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin);

//            workSheet.Cells[1, 1] = "From";
//            workSheet.Cells[1, 2] = "To";
//            workSheet.Cells[1, 3] = "Distance";
//            workSheet.Cells[1, 4] = "Mode";

//            int line = 2;

//            foreach (Link link in links)
//            {
//                workSheet.Cells[line, 1] = link.FromCity.Name;
//                workSheet.Cells[line, 2] = link.ToCity.Name;
//                workSheet.Cells[line, 3] = link.Distance;
//                workSheet.Cells[line, 4] = link.TransportMode;
//                line++;
//            }

//            excelApp.DisplayAlerts = false;
//            workBook.SaveAs(fileName);
//            workBook.Close();
//            excelApp.Quit();
//        }
//    }
//}
