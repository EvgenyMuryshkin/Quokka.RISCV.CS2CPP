using System;
using System.Collections.Generic;
using System.Text;

namespace DMA_RegisterTestSource
{
    public class DMA
    {
        public static DMA Instance { get; set; } = new DMA();

        public virtual int Counter { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            while(true)
            {
                DMA.Instance.Counter++;
            }
        }
    }
}
