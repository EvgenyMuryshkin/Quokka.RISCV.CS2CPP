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
        public FSTextFile ExtenalsImport(IEnumerable<ExternalDataRecord> data)
        {
            var content = new StringBuilder();
            foreach (var item in data)
            {
                var address = item.Segment << 24;
                content.AppendLine($"#define {item.SoftwareName} (*(volatile uint32_t*)0x{address.ToString("X8")})");
            }

            return new FSTextFile()
            {
                Name = "externals.h",
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
    }
}
