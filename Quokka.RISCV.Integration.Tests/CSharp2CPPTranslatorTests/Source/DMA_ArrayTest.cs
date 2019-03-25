using System;
using System.Collections.Generic;
using System.Text;

namespace DMA_ArrayTestSource
{
    public class DMA
    {
        public static DMA Instance { get; set; } = new DMA();

        public virtual int Result { get; set; }

        public virtual byte[] Array { get; set; } = new byte[16];
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            for (byte i = 0; i < 16; i++)
            {
                DMA.Instance.Array[i] = i;
            }

            int result = 0;
            for (byte i = 0; i < 16; i++)
            {
                result += DMA.Instance.Array[i];
            }

            DMA.Instance.Result = result;
        }
    }
}
