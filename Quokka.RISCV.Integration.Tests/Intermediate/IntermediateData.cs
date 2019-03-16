using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Tests.Intermediate
{
    class IntermediateData
    {
        static string SolutionFolder(string start)
        {
            if (!Directory.Exists(start))
                return null;

            if (Directory.EnumerateFiles(start, "*.sln").Any())
                return start;

            return SolutionFolder(Path.GetDirectoryName(start));
        }

        public static string IntegrationTestsFolder => Path.Combine(
            SolutionFolder(Environment.CurrentDirectory),
            "Quokka.RISCV.Integration.Tests");

        public static string IntermediateFolder => Path.Combine(
            IntegrationTestsFolder,
            "Intermediate");

        public static string FirmwareFolder => Path.Combine(
            IntermediateFolder, 
            "Firmware");

        public static string HardwareFolder => Path.Combine(
            IntermediateFolder, 
            "Hardware");

        static HashSet<string> Allowed = new HashSet<string>() { FirmwareFolder, HardwareFolder };

        static void Clean(string path)
        {
            if (!Allowed.Contains(path))
                throw new Exception($"Only for intermediate data");

            foreach (var file in Directory.EnumerateFiles(path).ToList())
            {
                File.Delete(file);
            }
        }

        public static void SaveFirmware(FSSnapshot snapshot)
        {
            Clean(FirmwareFolder);
            var m = new FSManager(FirmwareFolder);
            m.SaveSnapshot(snapshot);
        }

        public static void SaveHardware(FSSnapshot snapshot)
        {
            Clean(HardwareFolder);
            var m = new FSManager(HardwareFolder);
            m.SaveSnapshot(snapshot);
        }
    }
}
