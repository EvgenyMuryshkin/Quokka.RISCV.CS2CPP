using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayDeclarationTestSource
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public byte U8Result { get; set; }
        public sbyte S8Result;
        public char C16Result;
        public ushort U16Result;
        public short S16Result;
        public uint U32Result;
        public int S32Result;
    }

    public static class Firmware
    {
        static byte U8Result = byte.MaxValue;
        static byte[] U8Buff = new byte[16];

        static sbyte S8Result = sbyte.MinValue;
        static sbyte[] S8Buff = new sbyte[16];

        static char C16Result = char.MaxValue;
        static char[] C16Buff = new char[16];

        static ushort U16Result = 42;
        static ushort[] U16Buff = new ushort[16];

        static short S16Result = -42;
        static short[] S16Buff = new short[16];

        static uint U32Result = 0;
        static uint[] U32Buff = new uint[16];

        static int S32Result = 0;
        static int[] S32Buff = new int[16];

        static void InitData()
        {
            for (int i = 0; i < 16; i++)
            {
                int value = -40000 + 5000 * i + i;
                U8Buff[i] = (byte)value;
                S8Buff[i] = (sbyte)value;
                C16Buff[i] = (char)value;
                U16Buff[i] = (ushort)value;
                S16Buff[i] = (short)value;
                U32Buff[i] = (uint)value;
                S32Buff[i] = value;
            }
        }

        static void CalculateResult()
        {
            for (int i = 0; i < 16; i++)
            {
                U8Result += U8Buff[i];
                S8Result += S8Buff[i];
                C16Result += C16Buff[i];
                U16Result += U16Buff[i];
                S16Result += S16Buff[i];
                U32Result += U32Buff[i];
                S32Result += S32Buff[i];
            }
        }

        static void SetResult()
        {
            SOC.Instance.U8Result = U8Result;
            SOC.Instance.S8Result = S8Result;
            SOC.Instance.C16Result = C16Result;
            SOC.Instance.U16Result = U16Result;
            SOC.Instance.S16Result = S16Result;
            SOC.Instance.U32Result = U32Result;
            SOC.Instance.S32Result = S32Result;
        }

        public static void EntryPoint()
        {
            InitData();
            CalculateResult();
            SetResult();
        }
    }
}
