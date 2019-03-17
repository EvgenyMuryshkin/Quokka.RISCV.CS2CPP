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

        async Task CompileFromIntermediate()
        {
            var context = RISCVIntegration.DefaultContext(TestPath.FirmwareSourceFolder);

            var result = await RISCVIntegrationClient.Run(context);
            Assert.IsNotNull(result);

            IntermediateData.SaveFirmwareOutput(result.ResultSnapshot);
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

            var result = tx.Run(source);

            var generatedSourceFiles = result.Files.Where(f => Path.GetExtension(f.Name).ToLower() == ".cpp").ToList();
            var generatedHeaderFiles = result.Files.Where(f => Path.GetExtension(f.Name).ToLower() == ".h").ToList();

            result.Merge(firmwareTemplates, f => !f.Name.Contains("template"));

            var dmaGenerator = new DMAGenerator();
            result.Add(dmaGenerator.DMAImport(Enumerable.Empty<DMARecord>()));

            var firmwareTemplate = firmwareTemplates.Get<FSTextFile>("firmware.template.cpp");
            var firmwareMap = new Dictionary<string, string>()
            {
                { "FIRMWARE_INCLUDES", string.Join(Environment.NewLine, generatedHeaderFiles.Select(f => $"#include \"{f.Name}\""))},
                { "FIRMWARE_CODE", $"{entryPoint.Method.DeclaringType.Namespace}::{entryPoint.Method.DeclaringType.Name}::{entryPoint.Method.Name}();" },
            };
            result.Add("firmware.cpp", textReplacer.ReplaceToken(firmwareTemplate.Content, firmwareMap));

            var makefileTemplate = firmwareTemplates.Get<FSTextFile>("Makefile.template");
            var makefileMap = new Dictionary<string, string>()
            {
                { "SOURCES_LIST", string.Join(" ", generatedSourceFiles.Select(f => f.Name)) }
            };
            result.Add("Makefile", textReplacer.ReplaceToken(makefileTemplate.Content, makefileMap));

            IntermediateData.SaveFirmwareSource(result);

            await CompileFromIntermediate();
        }


        [TestMethod]
        public async Task CompileFromIntermediateTest()
        {
            await CompileFromIntermediate();
        }

        [TestMethod]
        public async Task BasicTest()
        {
            await TranslateSourceFiles(
                new []
                {
                    LoadSource("BasicTest.cs")
                },
                UnitTests.BasicTest.EntryPoint
                );
        }
    }
}
