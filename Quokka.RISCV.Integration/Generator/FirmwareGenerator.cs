using Quokka.RISCV.Integration.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Generator
{
    public class FirmwareGenerator
    {
        public FSTextFile FromTemplate(string templateContent)
        {
            return new FSTextFile()
            {
                Name = "firmware.c",
                Content = templateContent
            };
        }
    }
}
