using System;
using System.Collections.Generic;
using System.Text;

namespace DataDeclarationTestSource
{
    public class DMA
    {
        public static DMA Instance { get; set; } = new DMA();

        public virtual int Result { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            byte b1 = 10, b2 = 20;
            int i1 = 0, i2 = 100;
            uint ui1 = 10000;

            DMA.Instance.Result = (int)((b1 + b2) * (i1 - i2) + ui1);
        }
    }
}
