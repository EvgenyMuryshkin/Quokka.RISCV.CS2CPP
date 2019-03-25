using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Quokka.RISCV.Integration.Generator
{
    public class TextReplacer
    {
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

        public string ReplaceToken(string hardwareTemplate, Dictionary<string, string> replacers)
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
    }
}
