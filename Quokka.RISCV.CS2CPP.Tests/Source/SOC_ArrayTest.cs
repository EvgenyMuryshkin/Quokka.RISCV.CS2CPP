using System;
using System.Collections.Generic;
using System.Text;

namespace SOC_ArrayTestSource
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual int Result { get; set; }

        public virtual byte[] Array { get; set; } = new byte[16];
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            for (byte i = 0; i < 16; i++)
            {
                SOC.Instance.Array[i] = i;
            }

            int result = 0;
            for (byte i = 0; i < 16; i++)
            {
                result += SOC.Instance.Array[i];
            }

            SOC.Instance.Result = result;
        }
    }
}
