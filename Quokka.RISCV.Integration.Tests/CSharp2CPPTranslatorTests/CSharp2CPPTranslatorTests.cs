using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.CS2CPP.Translator;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator;
using Quokka.RISCV.Integration.Generator.DMA;
using Quokka.RISCV.Integration.Tests.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quokka.RISCV.Integration.Tests.CSharp2CTranslatorTests
{
    [TestClass]
    public class CSharp2CPPTranslatorTests
    {
        FSTextFile LoadSource(string path) => new FSTextFile() { Name = path, Content = File.ReadAllText(Path.Combine(TestPath.SourcePath, path)) };

        async Task<FSSnapshot> CompileFromIntermediate()
        {
            var context = RISCVIntegration.DefaultContext(TestPath.FirmwareSourceFolder);

            var result = await RISCVIntegrationClient.Run(context);
            Assert.IsNotNull(result);

            return result.ResultSnapshot;
        }

        async Task TranslateSourceFiles(
            IEnumerable<FSTextFile> files,
            Action entryPoint
            )
        {
            var textReplacer = new TextReplacer();
            var generator = new IntegrationGenerator();

            var firmwareTemplates = IntegrationTemplatesLoader.FirmwareTemplates;
            var hardwareTemplates = IntegrationTemplatesLoader.HardwareTemplates;

            var tx = new CSharp2CPPTranslator();
            var source = new FSSnapshot();
            source.Files.AddRange(files);

            tx.Run(source);

            var firmwareSource = tx.Result;

            var dmaGenerator = new DMAGenerator();
            var dmaRecords = new List<DMARecord>();

            // default code block
            dmaRecords.Add(new DMARecord()
            {
                DataType = typeof(uint),
                SegmentBits = 12,
                HardwareName = "l_mem",
                SoftwareName = "l_mem",
                Segment = 0,
                Depth = 512,
                Template = "memory32"
            });

            uint seg = 0x800;

            foreach (var d in tx.DMA)
            {
                var segment = d.Address;
                if (segment == 0)
                {
                    segment = seg;
                    seg++;
                }

                var templatesMap = new Dictionary<Type, string>()
                {
                    { typeof(byte), "memory8" },
                    { typeof(sbyte), "memory8" },
                    { typeof(ushort), "memory16" },
                    { typeof(short), "memory16" },
                    { typeof(int), "memory32" },
                    { typeof(uint), "memory32" },
                };
                
                if (d.Length > 0 && !templatesMap.ContainsKey(d.Type))
                {
                    throw new Exception($"No template found for {d.Type}");
                }

                var template = d.Length > 0 ? templatesMap[d.Type] : "register";

                var rec = new DMARecord()
                {
                    SegmentBits = 12,
                    SoftwareName = d.Name,
                    HardwareName = d.Name,
                    DataType = d.Type,
                    Depth = (uint)d.Length,
                    Segment = segment,
                    Template = template,
                };

                dmaRecords.Add(rec);
            }

            firmwareSource.Add(dmaGenerator.DMAImport(dmaRecords));

            var generatedSourceFiles = firmwareSource.Files.Where(f => Path.GetExtension(f.Name).ToLower() == ".cpp").ToList();
            var generatedHeaderFiles = firmwareSource.Files.Where(f => Path.GetExtension(f.Name).ToLower() == ".h").ToList();

            firmwareSource.Merge(firmwareTemplates, f => !f.Name.Contains("template"));

            var firmwareTemplate = firmwareTemplates.Get<FSTextFile>("firmware.template.cpp");
            var firmwareMap = new Dictionary<string, string>()
            {
                { "FIRMWARE_INCLUDES", string.Join(Environment.NewLine, generatedHeaderFiles.Select(f => $"#include \"{f.Name}\""))},
                { "FIRMWARE_CODE", $"{entryPoint.Method.DeclaringType.Namespace}::{entryPoint.Method.DeclaringType.Name}::{entryPoint.Method.Name}();" },
            };
            firmwareSource.Add("firmware.cpp", textReplacer.ReplaceToken(firmwareTemplate.Content, firmwareMap));

            var makefileTemplate = firmwareTemplates.Get<FSTextFile>("Makefile.template");
            var makefileMap = new Dictionary<string, string>()
            {
                { "SOURCES_LIST", string.Join(" ", generatedSourceFiles.Select(f => f.Name)) }
            };
            firmwareSource.Add("Makefile", textReplacer.ReplaceToken(makefileTemplate.Content, makefileMap));

            IntermediateData.SaveFirmwareSource(firmwareSource);

            var firmwareOutput = await CompileFromIntermediate();

            IntermediateData.SaveFirmwareOutput(firmwareOutput);

            // generat verilog

            var hardwareTemplate = hardwareTemplates.Get<FSTextFile>("hardware.template.v").Content;

            // memory init file
            var binFile = firmwareOutput.Get<FSBinaryFile>("firmware.bin");
            Assert.IsNotNull(binFile);

            var replacers = new Dictionary<string, string>();

            var words = TestTools.ReadWords(binFile.Content).ToList();
            var memInit = generator.MemInit(words, "l_mem", 512);

            replacers["MEM_INIT"] = memInit;

            // data declarations
            replacers["DATA_DECL"] = generator.DataDeclaration(dmaRecords);

            // data control signals
            var templates = new IntegrationTemplates();
            foreach (var t in hardwareTemplates.Files.OfType<FSTextFile>())
            {
                templates.Templates[t.Name] = t.Content;
            }

            replacers["DATA_CTRL"] = generator.DataControl(dmaRecords, templates);
            replacers["MEM_READY"] = generator.MemReady(dmaRecords);
            replacers["MEM_RDATA"] = generator.MemRData(dmaRecords);

            hardwareTemplate = textReplacer.ReplaceToken(hardwareTemplate, replacers);

            var hardwareSource = new FSSnapshot();
            hardwareSource.Add("hardware.v", hardwareTemplate);

            IntermediateData.SaveHardwareSource(hardwareSource);
        }


        [TestMethod]
        public async Task CompileFromIntermediateTest()
        {
            var firmwareSnapshot = await CompileFromIntermediate();

            IntermediateData.SaveFirmwareOutput(firmwareSnapshot);
        }

        [TestMethod]
        public async Task BasicTest()
        {
            await TranslateSourceFiles(
                new []
                {
                    LoadSource("BasicTest.cs")
                },
                BasicTestSource.Firmware.EntryPoint
                );
        }
    }
}
