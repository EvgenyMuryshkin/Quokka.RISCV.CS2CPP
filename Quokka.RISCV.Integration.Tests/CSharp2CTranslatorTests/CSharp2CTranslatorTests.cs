using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.CS2CPP.Translator;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System.IO;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.CSharp2CTranslatorTests
{
    [TestClass]
    public class CSharp2CTranslatorTests
    {
        string SourcePath => @"C:\code\Quokka.RISCV.Docker.Server\Quokka.RISCV.Integration.Tests\CSharp2CTranslatorTests\Source";

        FSTextFile LoadSource(string path) => new FSTextFile() { Name = path, Content = File.ReadAllText(Path.Combine(SourcePath, path)) };

        [TestMethod]
        public void BasicTest()
        {
            var tx = new CSharp2CPPTranslator();
            var source = new FSSnapshot();
            source.Files.Add(LoadSource("BasicTest.cs"));

            var result = tx.Run(source);

            var fsm = new FSManager(@"C:\code\Quokka.RISCV.Docker.Server\Quokka.RISCV.Integration.Tests\Client\Blinker\Source");
            fsm.SaveSnapshot(result);
        }
    }
}
