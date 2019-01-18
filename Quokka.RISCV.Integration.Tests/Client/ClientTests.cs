using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quokka.RISCV.Docker.Server.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public async Task ClientTest_Windows()
        {
            if (!Debugger.IsAttached)
                Assert.Inconclusive("Run local service and debug the this test");

            var testDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "TestDataWindows");

            var context = new RISCVIntegrationClientContext()
                .WithPort(15001)
                .WithExtensionClasses(new ExtensionClasses().Text("cmd"))
                .WithRootFolder(testDataRoot)
                .WithAllFiles()
                .WithOperations(new CmdInvocation("1.cmd"))
                .TakeModifiedFiles()
                ;

            var result = await RISCVIntegrationClient.Run(context);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ClientTest_Docker()
        {
            if (!Debugger.IsAttached)
                Assert.Inconclusive("Run local service and debug the this test");

            var testDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "TestDataDocker");

            var context = new RISCVIntegrationClientContext()
                .WithPort(15000)
                .WithExtensionClasses(new ExtensionClasses().Text("sh"))
                .WithRootFolder(testDataRoot)
                .WithAllFiles()
                .WithOperations(
                    new BashInvocation("chmod 777 ./1.sh"),
                    new ResetRules(),
//                    new BashInvocation("mkdir output"),
                    new BashInvocation("./1.sh")
                )
                .TakeModifiedFiles()
                ;

            var result = await RISCVIntegrationClient.Run(context);

            Assert.IsNotNull(result);
        }
    }
}
