using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Generator.DMA;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Quokka.RISCV.Integration.Generator
{
    public class IntegrationTemplates
    {
        public Dictionary<string, string> Templates { get; private set; } = new Dictionary<string, string>();
    }

    public class IntegrationGenerator
    {
        public FSTextFile Firmware(string content)
        {
            return new FirmwareGenerator().FromTemplate(content);
        }

        public string DataDeclaration(DMARecord record)
        {
            if (record.Depth == 0 )
            {
                if (record.Width == 1)
                {
                    return $"reg {record.HardwareName};";
                }
                else
                {
                    return $"reg [{record.Width - 1} : 0] {record.HardwareName};";
                }
            }
            else
            {
                if (record.Width == 1)
                {
                    return $"reg {record.HardwareName}[0 : {record.Depth}];";
                }
                else
                {
                    return $"reg [{record.Width - 1} : 0] {record.HardwareName}[0 : {record.Depth - 1}];";
                }
            }
        }

        public string DataDeclaration(List<DMARecord> externalData)
        {
            return string.Join("", externalData.Select(d => $"{DataDeclaration(d)}{Environment.NewLine}"));
        }

        public string DataControl(
            DMARecord record,
            IntegrationTemplates templates)
        {
            var map = new Dictionary<string, string>();
            map["NAME"] = record.HardwareName;
            map["SEG"] = record.Segment.ToString("X2");
            map["be_3"] = record.Width > 24 ? "" : "//";
            map["be_2"] = record.Width > 16 ? "" : "//";
            map["be_1"] = record.Width > 8 ? "" : "//";
            map["be_0"] = record.Width > 0 ? "" : "//";
            map["WIDTH"] = record.Width.ToString();
            map["HIGH"] = (record.Width - 1).ToString();

            var template = templates.Templates[record.Template];

            foreach (var pair in map)
            {
                var token = $"{{{pair.Key}}}";
                template = template.Replace(token, pair.Value);
            }

            return template;
        }

        public string DataControl(
            List<DMARecord> externalData,
            IntegrationTemplates templates)
        {
            return string.Join("", externalData.Select(d => $"{DataControl(d, templates)}{Environment.NewLine}"));
        }

        public string MemReady(List<DMARecord> externalData)
        {
            return string.Join(" || ", externalData.Select(d => $"{d.HardwareName}_ready"));
        }

        public string MemRData(List<DMARecord> externalData)
        {
            if (!externalData.Any())
                return "32'b0";

            var first = externalData.First();
            var source = first.Depth != 0 ? $"{first.HardwareName}_rdata" : first.HardwareName;

            return $"{first.HardwareName}_ready ? {source} : {MemRData(externalData.Skip(1).ToList())}";
        }

        public string MemInit(List<uint> words, string memName, int wordsCount)
        {
            var init = Enumerable
                .Range(0, wordsCount)
                .Select(idx => $"/*{(idx * 4).ToString("X4")}*/{memName}[{idx}] = 32'h{ (idx < words.Count ? words[idx] : 0).ToString("X8")};{Environment.NewLine}");

            return string.Join("", init);
        }
    }
}
