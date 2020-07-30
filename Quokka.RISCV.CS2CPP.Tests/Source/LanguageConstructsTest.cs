using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageConstructsTestSource
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual int Value { get; set; }

        public virtual byte[] Array { get; set; } = new byte[512];
    }

    public static class Firmware
    {
        static void IfStatement(int counter)
        {
            if (counter < 5)
                SOC.Instance.Value = 0;
            else if (counter < 8)
                SOC.Instance.Value = 1;
            else
                SOC.Instance.Value = counter + 10;
        }

        static void SwitchStatement(int counter)
        {
            switch (counter)
            {
                case 0:
                    SOC.Instance.Value = 0;
                    break;
                case 1:
                    SOC.Instance.Value = 0;
                    break;
                case 2:
                case 3:
                    SOC.Instance.Value = 2;
                    break;
                default:
                    SOC.Instance.Value = counter * 3;
                    break;
            }
        }

        static void LoopStatements(int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                SOC.Instance.Value *= i;
            }

            for (int i = counter; i >= 0; i--)
            {
                SOC.Instance.Value /= i;
            }

            while (counter-->= 0)
            {
                SOC.Instance.Value += counter;
            }

            do
            {
                SOC.Instance.Value += counter;
            }
            while (counter++ < 10);
        }

        public static void EntryPoint()
        {
            int counter = 0;
            while(counter < 10)
            {
                IfStatement(counter);
                SwitchStatement(counter);
                LoopStatements(counter);
            }
        }
    }
}
