using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class CWriter
    {
        StringBuilder _builder = new StringBuilder();
        int Indent = 0;
        string FormatIndent => string.Join("", Enumerable.Range(0, Indent).Select(i => "\t"));

        public void OpenBlock()
        {
            AppendLine("{");
            Indent++;
        }

        public void CloseBlock()
        {
            Indent--;
            AppendLine("}");
        }

        public void AppendLine(string value)
        {
            var lines = value.Split(Environment.NewLine);
            var indent = FormatIndent;

            foreach (var l in lines)
            {
                _builder.AppendLine($"{indent}{l}");
            }
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
