using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public static class BasicTest
    {
        public static void _main()
        {
            int counter = 0;
            var tmp = 0.5;

            while(counter < 1024)
            {
                counter = counter + 1;
                tmp = tmp * 2;
            }
        }
    }
}
