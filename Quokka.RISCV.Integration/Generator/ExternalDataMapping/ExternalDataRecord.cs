using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Generator.ExternalDataMapping
{
    public class ExternalDataRecord
    {
        public string SoftwareName { get; set; }
        public string HardwareName { get; set; }
        public uint Segment { get; set; }
        public uint Width { get; set; } = 1;
        public uint Depth { get; set; }
        public string Template { get; set; }
    }
}
