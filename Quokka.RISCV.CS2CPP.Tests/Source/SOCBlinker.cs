using System;
using System.Collections.Generic;
using System.Text;

namespace SOCBlinker
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            while(true)
            {
                uint counter = SOC.Instance.Counter;
                counter++;
                SOC.Instance.Counter = counter;
            }
        }
    }
}
