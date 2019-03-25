using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{
    public class ClassMembersCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            Invoke<ClassMethodCModelVisitor>(model);
        }
    }
}
