using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using Quokka.RISCV.Integration.Tests.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.Tools
{
    class IntermediateData
    {
        static HashSet<string> Allowed = new HashSet<string>() {
            TestPath.FirmwareSourceFolder,
            TestPath.FirmwareOutputFolder,
            TestPath.HardwareSourceFolder
        };

        static void Clean(string path)
        {
            if (!Allowed.Contains(path))
                throw new Exception($"Only for intermediate data");

            if (!Directory.Exists(path))
                return;

            foreach (var file in Directory.EnumerateFiles(path).ToList())
            {
                File.Delete(file);
            }
        }

        public static void SaveFirmwareSource(FSSnapshot snapshot)
        {
            Clean(TestPath.FirmwareSourceFolder);
            var m = new FSManager(TestPath.FirmwareSourceFolder);
            m.SaveSnapshot(snapshot);
        }
        
        public static void SaveFirmwareOutput(FSSnapshot snapshot)
        {
            Clean(TestPath.FirmwareOutputFolder);
            var m = new FSManager(TestPath.FirmwareOutputFolder);
            m.SaveSnapshot(snapshot);
        }

        public static void SaveHardwareSource(FSSnapshot snapshot)
        {
            Clean(TestPath.HardwareSourceFolder);
            var m = new FSManager(TestPath.HardwareSourceFolder);
            m.SaveSnapshot(snapshot);
        }
    }
}
