using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.CS2CPP.CodeWriters.Tools
{
    public class CodeWriterContext
    {
        public string FileName { get; set; }

        public NamespaceCPPModel Namespace { get; set; }
        public ClassCPPModel Class { get; set; }
        public MethodCPPModel Method { get; set; }
    }
}
