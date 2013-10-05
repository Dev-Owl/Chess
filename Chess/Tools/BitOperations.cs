using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chess.Tools
{
    class BitOperations
    {
        public static int NumberOfSetBits(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555UL);
            i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
            return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }

        public static UInt64 SetBit(int BitToset)
        {
            UInt64 result = 1;
            result = result << BitToset;
            return result;
        }

        public static UInt64 MostSigBitSet(UInt64 value)
        {
            if (value == 0)
            {
                return 0;
            }
            value |= (value >> 1);
            value |= (value >> 2);
            value |= (value >> 4);
            value |= (value >> 8);
            value |= (value >> 16);

            return (value & ~(value >> 1));
        }

        public static int MostSigExponent(UInt64 value)
        {
            if (value == 0)
            {
                return 0;
            }
            return (int)Math.Log(MostSigBitSet(value), 2);
        }

        public static string CreateHumanString(UInt64 DataToDump)
        {
            string binaryValue = Convert.ToString((long)DataToDump, 2).PadLeft(64, '0');
            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < 64; ++i)
            {
                if (i % 8 == 0 && i != 0)
                    resultBuilder.Append(Environment.NewLine);

                resultBuilder.Append(binaryValue[i]);
            }

            return resultBuilder.ToString();
        }

        public static void DumpArrayToFile(int[,] Array, string DumpPath)
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
