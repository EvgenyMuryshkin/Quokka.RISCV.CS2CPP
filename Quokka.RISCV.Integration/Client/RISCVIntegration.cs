using Quokka.RISCV.Integration.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Client
{
    public class RISCVIntegration
    {
        public static RISCVIntegrationContext DefaultContext(string rootPath)
        {
            var context = new RISCVIntegrationContext()
                .WithPort(15000)
                .WithExtensionClasses(
                    new ExtensionClasses()
                        .Text("")
                        .Text("lds")
                        .Text("s")
                        .Text("c")
                        .Text("cpp")
                        .Text("h")
                        .Binary("bin")
                        .Binary("elf")
                        .Text("map")
                )
                .WithRootFolder(rootPath)
                .WithAllRegisteredFiles()
                .WithOperations(new BashInvocation("make firmware.bin"))
                .TakeModifiedFiles();

            return context;
        }
    }
}
