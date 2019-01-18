using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quokka.RISCV.Docker.Server.Tests
{
    [TestClass]
    public class RulesManagerTests
    {
        [TestMethod]
        public void AllFilesTest()
        {
            var id = Guid.NewGuid();
            using (var tc = new Toolchain(id))
            {
                tc.SetupRules(new [] { new AllFilesRule() });
                File.WriteAllText(Path.Combine(tc.RootPath, "1.txt"), "content");
                File.WriteAllBytes(Path.Combine(tc.RootPath, "1.bin"), new byte[] { 128 });

                var classes = new ExtensionClasses().Text("txt").Binary("bin");

                var result = tc.LoadSnapshot(classes);

                Assert.AreEqual(2, result.Files.Count);

                Assert.AreEqual(1, result.Files.OfType<FSTextFile>().Count());
                Assert.AreEqual(1, result.Files.OfType<FSBinaryFile>().Count());
            }
        }

        [TestMethod]
        public void ExtensionMatchFilesTest()
        {
            var id = Guid.NewGuid();
            using (var tc = new Toolchain(id))
            {
                tc.SetupRules(new[] { new ExtensionMatchFilesRule() { Extensions = new List<string>() { "txt" } } });
                File.WriteAllText(Path.Combine(tc.RootPath, "1.txt"), "content");
                File.WriteAllBytes(Path.Combine(tc.RootPath, "1.bin"), new byte[] { 128 });

                var classes = new ExtensionClasses().Text("txt").Binary("bin");

                var result = tc.LoadSnapshot(classes);

                Assert.AreEqual(1, result.Files.Count);

                Assert.AreEqual(1, result.Files.OfType<FSTextFile>().Count());
                Assert.AreEqual(0, result.Files.OfType<FSBinaryFile>().Count());
            }
        }

        [TestMethod]
        public void RegexMatchFilesTest()
        {
            var id = Guid.NewGuid();
            using (var tc = new Toolchain(id))
            {
                tc.SetupRules(new[] { new RegexMatchFilesRule() { Patterns = new List<string>() { "bin" } } });
                File.WriteAllText(Path.Combine(tc.RootPath, "1.txt"), "content");
                File.WriteAllBytes(Path.Combine(tc.RootPath, "1.bin"), new byte[] { 128 });

                var classes = new ExtensionClasses().Text("txt").Binary("bin");

                var result = tc.LoadSnapshot(classes);

                Assert.AreEqual(1, result.Files.Count);

                Assert.AreEqual(0, result.Files.OfType<FSTextFile>().Count());
                Assert.AreEqual(1, result.Files.OfType<FSBinaryFile>().Count());
            }
        }

        [TestMethod]
        public void ModifiedFilesTest()
        {
            var id = Guid.NewGuid();
            using (var tc = new Toolchain(id))
            {
                // write some content before setup rules
                File.WriteAllText(Path.Combine(tc.RootPath, "1.txt"), "content");
                File.WriteAllText(Path.Combine(tc.RootPath, "2.txt"), "content");

                tc.SetupRules(new[] { new ModifiedFilesRule() });
                File.WriteAllBytes(Path.Combine(tc.RootPath, "1.bin"), new byte[] { 128 });
                File.Move(Path.Combine(tc.RootPath, "2.txt"), Path.Combine(tc.RootPath, "3.txt"));

                var classes = new ExtensionClasses().Text("txt").Binary("bin");

                var result = tc.LoadSnapshot(classes);

                Assert.AreEqual(2, result.Files.Count);

                Assert.AreEqual(1, result.Files.OfType<FSTextFile>().Count());
                var textFile = result.Files.OfType<FSTextFile>().First();
                Assert.AreEqual("3.txt", textFile.Name);

                Assert.AreEqual(1, result.Files.OfType<FSBinaryFile>().Count());
            }
        }
    }
}
