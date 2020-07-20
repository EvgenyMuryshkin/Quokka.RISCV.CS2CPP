using System;
using System.Collections.Generic;
using System.Text;

namespace SOCBlinker
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual byte Blinker { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            uint counter = 0;
            while(true)
            {
                counter++;
                SOC.Instance.Blinker = (byte)(counter >> 24);
            }
        }
    }
}
