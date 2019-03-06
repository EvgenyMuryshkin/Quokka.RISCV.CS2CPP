using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class ClassMembersCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            Invoke<ClassMethodCModelVisitor>(model);
        }
    }
}
