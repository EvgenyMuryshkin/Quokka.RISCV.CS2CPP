using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System;
using System.IO;
using System.Linq;

namespace Quokka.RISCV.Docker.Server.Tests
{
    [TestClass]
    public class ToolchainTests
    {
        [TestMethod]
        public void TempFolder()
        {
            var id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            using (var tc = new Toolchain(id))
            {
                Assert.IsTrue(Directory.Exists(tempFolder));
            }

            Assert.IsFalse(Directory.Exists(tempFolder));
        }

        [TestMethod]
        public void SaveSnapshot()
        {
            using (var tc = new Toolchain(Guid.NewGuid()))
            {
                var fs = new FSSnapshot();
                fs.Files.Add(new FSTextFile()
                {
                    Name = "1.txt",
                    Content = "abc"
                });

                fs.Files.Add(new FSBinaryFile()
                {
                    Name = "1.bin",
                    Content = new byte[] { 128 }
                });

                tc.SaveSnapshot(fs);

                Assert.AreEqual("abc", File.ReadAllText(Path.Combine(tc.RootPath, "1.txt")));
                Assert.AreEqual(128, File.ReadAllBytes(Path.Combine(tc.RootPath, "1.bin"))[0]);
            }
        }

        [TestMethod]
        public void ExecuteCommandTest_Success()
        {
            using (var tc = new Toolchain(Guid.NewGuid()))
            {
                var fs = new FSSnapshot();
                fs.Files.Add(new FSTextFile()
                {
                    Name = "1.cmd",
                    Content = "copy 1.txt 2.txt"
                });

                fs.Files.Add(new FSTextFile()
                {
                    Name = "1.txt",
                    Content = "content"
                });

                tc.SaveSnapshot(fs);
                tc.SetupRules(new[] { new ModifiedFilesRule() });

                tc.Invoke(new[]
                {
                    new CommandLineInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c 1.cmd"
                    }
                });

                var result = tc.LoadSnapshot(new ExtensionClasses().Text("txt"));
                Assert.AreEqual(1, result.Files.Count);

                var file = (FSTextFile)result.Files[0];
                Assert.AreEqual("2.txt", file.Name);
                Assert.AreEqual("content", file.Content);
            }
        }

        [TestMethod]
        public void ExecuteCommandTest_Fail()
        {
            using (var tc = new Toolchain(Guid.NewGuid()))
            {
                Assert.ThrowsException<Exception>(() =>
                {
                    tc.Invoke(new[]
                    {
                        new CommandLineInfo()
                        {
                            FileName = "cmd.exe",
                            Arguments = "/c 1.cmd"
                        }
                    });
                });
            }
        }
    }
}
