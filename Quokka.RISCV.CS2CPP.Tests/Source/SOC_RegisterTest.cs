using System;
using System.Collections.Generic;
using System.Text;

namespace SOC_RegisterTestSource
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual int Counter { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            while(true)
            {
                SOC.Instance.Counter++;
            }
        }
    }
}
