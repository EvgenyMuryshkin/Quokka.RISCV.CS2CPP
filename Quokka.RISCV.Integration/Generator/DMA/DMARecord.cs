using System;
using System.Text;

namespace Quokka.RISCV.Integration.Generator.DMA
{
    public class DMARecord
    {
        public int SegmentBits { get; set; } = 8;

        public string SoftwareName { get; set; }
        public string HardwareName { get; set; }
        public uint Segment { get; set; }
        public uint Depth { get; set; }
        public string Template { get; set; }
        public Type DataType { get; set; }

        public uint Width => DMATypeLookups.DataSize(DataType);
        public string CType => DMATypeLookups.CType(DataType);
    }
}
