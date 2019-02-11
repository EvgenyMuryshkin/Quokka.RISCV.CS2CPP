using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Translator.CSharp2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.CSharp2CTranslatorTests
{
    [TestClass]
    public class CSharp2CTranslatorTests
    {
        string SourcePath => @"C:\code\Quokka.RISCV.Docker.Server\Quokka.RISCV.Integration\Translator\CSharp2C\Source";

        FSTextFile LoadSource(string path) => new FSTextFile() { Name = path, Content = File.ReadAllText(Path.Combine(SourcePath, path)) };

        [TestMethod]
        public void BasicTest()
        {
            var tx = new CSharp2CTranslator();
            var source = new FSSnapshot();
            source.Files.Add(LoadSource("BasicTest.cs"));

            var result = tx.Run(source);
        }
    }
}
