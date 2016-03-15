using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader reader, char separationChar)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line.Split(separationChar);
            }
        }
    }
}
