using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.CS2CPP.Translator;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Generator;
using Quokka.RISCV.Integration.Generator.ExternalDataMapping;
using Quokka.RISCV.Integration.Tests.Intermediate;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.CSharp2CTranslatorTests
{
    [TestClass]
    public class CSharp2CTranslatorTests
    {
        string SourcePath => @"C:\code\Quokka.RISCV.Docker.Server\Quokka.RISCV.Integration.Tests\CSharp2CTranslatorTests\Source";

        FSTextFile LoadSource(string path) => new FSTextFile() { Name = path, Content = File.ReadAllText(Path.Combine(SourcePath, path)) };

        public void CompileFromIntermediate()
        {

        }

        [TestMethod]
        public void CompileFromIntermediateTest()
        {
            if (!Debugger.IsAttached)
                Assert.Inconclusive("For diagnostics only");

            CompileFromIntermediate();
        }

        [TestMethod]
        public void BasicTest()
        {
            var generator = new IntegrationGenerator();

            var firmwareTemplates = IntegrationTemplatesLoader.FirmwareTemplates;
            var hardwareTemplates = IntegrationTemplatesLoader.HardwareTemplates;

            var tx = new CSharp2CPPTranslator();
            var source = new FSSnapshot();
            source.Files.Add(LoadSource("BasicTest.cs"));

            var result = tx.Run(source);

            result.Merge(firmwareTemplates, f => !f.Name.Contains("template"));

            result.Add(generator.DMAImport(Enumerable.Empty<ExternalDataRecord>()));

            IntermediateData.SaveFirmware(result);
        }
    }
}
