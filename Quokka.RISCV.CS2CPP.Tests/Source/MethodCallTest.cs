﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MethodCallTestSource
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual int Value { get; set; }

        public virtual byte[] Array { get; set; } = new byte[512];
    }

    public static class Firmware
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
                SOC.Instance.Value = counter;
            }
        }
    }
}
