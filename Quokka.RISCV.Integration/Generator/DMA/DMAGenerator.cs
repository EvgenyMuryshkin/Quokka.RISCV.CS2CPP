using Quokka.RISCV.Integration.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Generator.DMA
{
    public class DMAGenerator
    {
        public FSTextFile DMAImport(IEnumerable<DMARecord> data)
        {
            var content = new StringBuilder();
            foreach (var item in data)
            {
                var address = item.Segment << 24;
                if (item.Depth == 0)
                {
                    content.AppendLine($"#define {item.SoftwareName} (*(volatile {item.CType}*)0x{address.ToString("X8")})");
                }
                else
                {
                    content.AppendLine($"#define {item.SoftwareName} ((volatile {item.CType}*)0x{address.ToString("X8")})");
                    content.AppendLine($"#define {item.SoftwareName}_size {item.Depth.ToString()}");
                }
            }

            return new FSTextFile()
            {
                Name = "dma.h",
                Content = content.ToString()
            };
        }
    }
}
