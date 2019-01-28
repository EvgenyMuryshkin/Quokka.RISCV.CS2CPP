using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator;
using Quokka.RISCV.Integration.Generator.ExternalDataMapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                Assert.Inconclusive("Run local service and debug this test");

            var testDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "TestDataWindows");

            var context = new RISCVIntegrationClientContext()
                .WithPort(15001)
                .WithExtensionClasses(new ExtensionClasses().Text("cmd"))
                .WithRootFolder(testDataRoot)
                .WithAllRegisteredFiles()
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
                Assert.Inconclusive("Run local service and debug this test");

            var testDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "TestDataDocker");

            var context = new RISCVIntegrationClientContext()
                .WithPort(15000)
                .WithExtensionClasses(new ExtensionClasses().Text("sh"))
                .WithRootFolder(testDataRoot)
                .WithAllRegisteredFiles()
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

        [TestMethod]
        public async Task ClientTest_Docker_TinyFPGA()
        {
            if (!Debugger.IsAttached)
                Assert.Inconclusive("Run local service and debug this test");

            var testDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "TinyFPGA-BX");

            var context = new RISCVIntegrationClientContext()
                .WithPort(15000)
                .WithExtensionClasses(
                    new ExtensionClasses()
                        .Text("sh")
                        .Text("")
                        .Text("lds")
                        .Text("s")
                        .Text("c")
                        .Binary("bin")
                        .Binary("elf")
                        .Text("map")
                )
                .WithRootFolder(testDataRoot)
                .WithAllRegisteredFiles()
                .WithOperations(
                    new BashInvocation("make firmware.bin")
                )
                .TakeModifiedFiles()
                ;

            var result = await RISCVIntegrationClient.Run(context);

            Assert.IsNotNull(result);

            var binFile = result.ResultSnapshot.Files.Find(f => f.Name == "firmware.bin");
            Assert.IsNotNull(binFile);
        }


        [TestMethod]
        public async Task ClientTest_Docker_Blinker()
        {
            //if (!Debugger.IsAttached)
            //    Assert.Inconclusive("Run local service and debug this test");

            var templateRoot = Path.Combine(Directory.GetCurrentDirectory(), "client", "Blinker", "Template");
            var sourceRoot = @"C:\code\Quokka.RISCV.Docker.Server\Quokka.RISCV.Integration.Tests\Client\Blinker\Source";

            var externalData = new List<ExternalDataRecord>()
            {
                new ExternalDataRecord() {
                    Segment = 0x00,
                    Width = 32,
                    Depth = 512,
                    SoftwareName = "l_mem",
                    HardwareName = "l_mem",
                    Template = "memory"
                },
                new ExternalDataRecord() {
                    Segment = 0x01,
                    Width = 32,
                    SoftwareName = "LED1",
                    HardwareName = "led1",
                    Template = "register"
                },
                new ExternalDataRecord() {
                    Segment = 0x02,
                    Width = 32,
                    SoftwareName = "LED2",
                    HardwareName = "led2",
                    Template = "register"
                },
                new ExternalDataRecord() {
                    Segment = 0x03,
                    Width = 32,
                    Depth = 64,
                    SoftwareName = "UART_TX",
                    HardwareName = "buff_uart_tx",
                    Template = "memory"
                },
            };

            var context = new RISCVIntegrationClientContext()
                .WithPort(15000)
                .WithExtensionClasses(
                    new ExtensionClasses()
                        .Text("")
                        .Text("lds")
                        .Text("s")
                        .Text("c")
                        .Text("h")
                        .Binary("bin")
                        .Binary("elf")
                        .Text("map")
                )
                .WithRootFolder(sourceRoot)
                .WithAllRegisteredFiles()
                .WithOperations(
                    new BashInvocation("make firmware.bin")
                )
                .TakeModifiedFiles()
                ;

            var firmwareTemplatePath = File.ReadAllText(Path.Combine(templateRoot, "firmware.template.c"));
            var generator = new IntegrationGenerator();
            context.SourceSnapshot.Files.Add(generator.ExtenalsImport(externalData));
            context.SourceSnapshot.Files.Add(generator.Firmware(firmwareTemplatePath));

            new FSManager(sourceRoot).SaveSnapshot(context.SourceSnapshot);

            var result = await RISCVIntegrationClient.Run(context);
            Assert.IsNotNull(result);

            var hardwareTemplatePath = Path.Combine(templateRoot, "hardware.template.v");
            var hardwareTemplate = File.ReadAllText(hardwareTemplatePath);

            // memory init file
            var binFile = (FSBinaryFile)result.ResultSnapshot.Files.Find(f => f.Name == "firmware.bin");
            Assert.IsNotNull(binFile);

            var replacers = new Dictionary<string, string>();

            var words = ReadWords(binFile.Content).ToList();
            var memInit = generator.MemInit(words, "l_mem", 512);

            replacers["MEM_INIT"] = memInit;

            // data declarations
            replacers["DATA_DECL"] = generator.DataDeclaration(externalData);

            // data control signals
            var templates = new IntegrationTemplates();
            templates.Templates["memory"] = File.ReadAllText(Path.Combine(templateRoot, "memory.template.v"));
            templates.Templates["register"] = File.ReadAllText(Path.Combine(templateRoot, "register.template.v"));

            replacers["DATA_CTRL"] = generator.DataControl(externalData, templates);
            replacers["MEM_READY"] = generator.MemReady(externalData);
            replacers["MEM_RDATA"] = generator.MemRData(externalData);

            hardwareTemplate = IntegrationGenerator.ReplaceToken(hardwareTemplate, replacers);

            File.WriteAllText(@"C:\code\picorv32\quartus\RVTest.v", hardwareTemplate);
        }

        IEnumerable<uint> ReadWords(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                using (var r = new BinaryReader(ms))
                {
                    while(r.BaseStream.Position != r.BaseStream.Length)
                    {
                        yield return r.ReadUInt32();
                    }
                }
            }
        }
    }
}
