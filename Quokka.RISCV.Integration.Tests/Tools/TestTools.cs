using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.Tools
{
    class TestTools
    {
        public static IEnumerable<uint> ReadWords(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                using (var r = new BinaryReader(ms))
                {
                    while (r.BaseStream.Position != r.BaseStream.Length)
                    {
                        yield return r.ReadUInt32();
                    }
                }
            }
        }
    }
}
