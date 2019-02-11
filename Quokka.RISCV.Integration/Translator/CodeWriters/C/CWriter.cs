using Quokka.RISCV.Integration.Translator.CodeModels.C;
using Quokka.RISCV.Integration.Translator.CSharp2C.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.RISCV.Integration.Translator.CodeWriters.C
{
    public class CWriter : CModelVisitor
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

        public override void DefaultVisit(CModel model)
        {
            throw new Exception($"Unsupported node type: {model.GetType().Name}: {model.ToString()}");
        }

        public override void VisitCIncludeModel(CIncludeModel model)
        {
            switch(model.Type)
            {
                case CIncludeModelType.System:
                    AppendLine($"include <{model.Name}>");
                    break;
                case CIncludeModelType.User:
                    AppendLine($"include \"{model.Name}\"");
                    break;
            }
        }

        public override void VisitCFileModel(CFileModel model)
        {
            foreach (var child in model.Children)
            {
                Visit(child);
            }
        }

        public static string Write(CModel model)
        {
            var writer = new CWriter();
            writer.Visit(model);
            return writer.ToString();
        }
    }
}
