using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public static class BasicTest
    {
        public static int increment(int value)
        {
            return value + 1;
        }

        public static void incrementByRef(ref int value)
        {
            value++;
        }

        public static void EntryPoint()
        {
            int counter = 0;

            while(counter < 1024)
            {
                counter = increment(counter);
                incrementByRef(ref counter);
            }
        }
    }
}
