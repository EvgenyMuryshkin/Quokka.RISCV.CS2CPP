using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.Tests.Tools;
using Quokka.RISCV.CS2CPP.Translator;
using Quokka.RISCV.Integration.Client;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Generator;
using Quokka.RISCV.Integration.Generator.SOC;
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
        FSTextFile LoadSource(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(TestPath.SourcePath, path);

            return new FSTextFile() { Name = path, Content = File.ReadAllText(path) };
        }

        async Task<FSSnapshot> CompileFromIntermediate()
        {
            var context = RISCVIntegration
                .DefaultContext(TestPath.FirmwareSourceFolder);

            if (!await RISCVIntegrationClient.HealthCheck(context.Endpoint))
            {
                throw new Exception($"RISCV toolchain not available");
            }

            var result = await RISCVIntegrationClient.Run(context);
            Assert.IsNotNull(result);

            return result.ResultSnapshot;
        }

        int SizeOfType(Type t)
        {
            var sizeOfMap = new Dictionary<Type, int>()
                {
                    { typeof(byte), sizeof(byte) },
                    { typeof(sbyte), sizeof(sbyte) },
                    { typeof(ushort), sizeof(ushort) },
                    { typeof(short), sizeof(short) },
                    { typeof(int), sizeof(int) },
                    { typeof(uint), sizeof(uint) },
                };

            if (!sizeOfMap.ContainsKey(t))
                throw new Exception($"SizeOf {t.Name} not registered");

            return sizeOfMap[t];
        }

        List<SOCRecord> ToSOCRecords(uint seg, IEnumerable<SOCResourceCPPModel> source)
        {
            var socRecords = new List<SOCRecord>();

            foreach (var d in source)
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

                var rec = new SOCRecord()
                {
                    SegmentBits = 12,
                    SoftwareName = d.Name,
                    HardwareName = d.Name,
                    DataType = d.Type,
                    Depth = (uint)d.Length,
                    Segment = segment,
                    Template = template,
                };

                socRecords.Add(rec);
            }

            return socRecords;
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

            var socGenerator = new SOCGenerator();
            var socRecords = new List<SOCRecord>();

            // default code block
            socRecords.Add(new SOCRecord()
            {
                DataType = typeof(uint),
                SegmentBits = 12,
                HardwareName = "firmware",
                SoftwareName = "firmware",
                Segment = 0,
                Depth = 512,
                Template = "memory32"
            });

            socRecords.AddRange(ToSOCRecords(0x800, tx.SOCResources));
            firmwareSource.Add(socGenerator.SOCImport(socRecords));

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
            var replacers = new Dictionary<string, string>();

            // memory init file
            replacers["MEM_INIT"] = "";
            foreach (var dma in socRecords)
            {
                var binFile = firmwareOutput.Get<FSBinaryFile>($"{dma.HardwareName}.bin");
                if(binFile != null)
                {
                    var words = TestTools.ReadWords(binFile.Content).ToList();
                    var memInit = generator.MemInit(words, dma.HardwareName, (int)dma.Depth, SizeOfType(dma.DataType));

                    replacers["MEM_INIT"] += memInit;
                }
                else
                {
                    var memInit = generator.MemInit(Enumerable.Range(0, (int)dma.Depth).Select(idx => 0UL).ToList(), dma.HardwareName, (int)dma.Depth, SizeOfType(dma.DataType));

                    replacers["MEM_INIT"] += memInit;
                }
            }

            // data declarations
            replacers["DATA_DECL"] = generator.DataDeclaration(socRecords);

            // data control signals
            var templates = new IntegrationTemplates();
            foreach (var t in hardwareTemplates.Files.OfType<FSTextFile>())
            {
                templates.Templates[t.Name] = t.Content;
            }

            replacers["DATA_CTRL"] = generator.DataControl(socRecords, templates);
            replacers["MEM_READY"] = generator.MemReady(socRecords);
            replacers["MEM_RDATA"] = generator.MemRData(socRecords);

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
        public async Task DataDeclarationTest()
        {
            await TranslateSourceFiles(
                new[]
                {
                    LoadSource("DataDeclarationTest.cs")
                },
                DataDeclarationTestSource.Firmware.EntryPoint
                );
        }

        [TestMethod]
        public async Task MethodCallTest()
        {
            await TranslateSourceFiles(
                new []
                {
                    LoadSource("MethodCallTest.cs")
                },
                MethodCallTestSource.Firmware.EntryPoint
                );
        }

        [TestMethod]
        public async Task SOC_ArrayTest()
        {
            await TranslateSourceFiles(
                new[]
                {
                    LoadSource("SOC_ArrayTest.cs")
                },
                SOC_ArrayTestSource.Firmware.EntryPoint
                );
        }

        [TestMethod]
        public async Task SOC_Register()
        {
            await TranslateSourceFiles(
                new[]
                {
                    LoadSource("SOC_RegisterTest.cs")
                },
                SOC_RegisterTestSource.Firmware.EntryPoint
                );
        }

        [TestMethod]
        public async Task MakeTest()
        {
            var context = RISCVIntegration
                .DefaultContext(TestPath.MakeFolder)
                .WithMakeTarget("bin");

            var tx = new CSharp2CPPTranslator();
            var source = new FSSnapshot();
            source.Files.Add(LoadSource("SOCBlinker.cs"));
            tx.Run(source);
            var firmwareSource = tx.Result;
            var socGenerator = new SOCGenerator();
            var socRecords = ToSOCRecords(0x800, tx.SOCResources);
            firmwareSource.Add(socGenerator.SOCImport(socRecords));
            IntermediateData.SaveToMake(firmwareSource);

            await RISCVIntegrationClient.Make(context);
        }
    }
}
