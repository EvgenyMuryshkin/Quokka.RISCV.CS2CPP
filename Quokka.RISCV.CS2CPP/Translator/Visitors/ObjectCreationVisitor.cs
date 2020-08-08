using Quokka.RISCV.CS2CPP.CodeModels.CPP;

namespace Quokka.RISCV.CS2CPP.Translator.Visitors
{
    class ObjectCreationVisitor : CSharp2CVisitor
    {
        public ExpressionCPPModel Expression { get; set; }

    }
}
