using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Chess.Tools
{
    class DumpArray
    {
        public static void DumpArrayToFile(int[,] Array,string DumpPath) 
        {
            using (StreamWriter sw = new StreamWriter(DumpPath))
            {
                    for (int y = 0; y < Array.GetLength(1); ++y)
                    {
                        for (int x = 0; x < Array.GetLength(0); ++x)
                        {

                            sw.Write(Array[x, y]);

                        }
                        sw.WriteLine("");
                    }
            }
        }
    }
}
