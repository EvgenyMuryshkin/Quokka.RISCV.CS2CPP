using Quokka.RISCV.Integration.DTO;
using Quokka.RISCV.Integration.Generator.ExternalDataMapping;
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
        public FSTextFile DMAImport(IEnumerable<ExternalDataRecord> data)
        {
            var content = new StringBuilder();
            foreach (var item in data)
            {
                var address = item.Segment << 24;
                if (item.Depth == 0)
                {
                    content.AppendLine($"#define {item.SoftwareName} (*(volatile {item.CType}*)0x{address.ToString("X8")})");
                }
                else
                {
                    content.AppendLine($"#define {item.SoftwareName} ((volatile {item.CType}*)0x{address.ToString("X8")})");
                    content.AppendLine($"#define {item.SoftwareName}_size {item.Depth.ToString()}");
                }
            }

            return new FSTextFile()
            {
                Name = "dma.h",
                Content = content.ToString()
            };
        }

        public FSTextFile Firmware(string content)
        {
            return new FirmwareGenerator().FromTemplate(content);
        }

        public static string ReplaceToken(
            string content,
            string token,
            string replacement)
        {
            var modified = Regex.Replace(
                content,
                $"(// BEGIN {token})" + "(.*)" + $"(// END {token})",
                $"$1{Environment.NewLine}{replacement}{Environment.NewLine}$3",
                RegexOptions.Singleline);

            return modified;
        }

        public string DataDeclaration(ExternalDataRecord record)
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

        public string DataDeclaration(List<ExternalDataRecord> externalData)
        {
            return string.Join("", externalData.Select(d => $"{DataDeclaration(d)}{Environment.NewLine}"));
        }

        public string DataControl(
            ExternalDataRecord record,
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
            List<ExternalDataRecord> externalData,
            IntegrationTemplates templates)
        {
            return string.Join("", externalData.Select(d => $"{DataControl(d, templates)}{Environment.NewLine}"));
        }

        public string MemReady(List<ExternalDataRecord> externalData)
        {
            return string.Join(" || ", externalData.Select(d => $"{d.HardwareName}_ready"));
        }

        public static string ReplaceToken(string hardwareTemplate, Dictionary<string, string> replacers)
        {
            // regex replace
            foreach (var pair in replacers)
            {
                hardwareTemplate = ReplaceToken(hardwareTemplate, pair.Key, pair.Value);
            }

            // token replace
            foreach (var pair in replacers)
            {
                var token = $"{{{pair.Key}}}";
                hardwareTemplate = hardwareTemplate.Replace(token, pair.Value);
            }

            return hardwareTemplate;
        }

        public string MemRData(List<ExternalDataRecord> externalData)
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
