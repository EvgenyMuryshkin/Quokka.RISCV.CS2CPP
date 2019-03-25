using Quokka.RISCV.Integration.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Generator
{
    public static class IntegrationTemplatesLoader
    {
        public static FSSnapshot HardwareTemplates
        {
            get
            {
                var result = new FSSnapshot();

                result.Add("hardware.template.v", Resources.hardware_template);
                result.Add("memory32", Resources.memory32_template);
                result.Add("memory16", Resources.memory16_template);
                result.Add("memory8", Resources.memory8_template);
                result.Add("register", Resources.register_template);

                return result;
            }
        }

        public static FSSnapshot FirmwareTemplates
        {
            get
            {
                var result = new FSSnapshot();

                result.Add("firmware.template.cpp", Resources.firmware_template);
                result.Add("start.S", Resources.start);
                result.Add("custom_ops.S", Resources.custom_ops);
                result.Add("irq.c", Resources.irq);
                result.Add("Makefile.template", Resources.Makefile);
                result.Add("plumbing.h", Resources.plumbing);
                result.Add("sections.lds", Resources.sections);

                return result;
            }
        }
    }
}
