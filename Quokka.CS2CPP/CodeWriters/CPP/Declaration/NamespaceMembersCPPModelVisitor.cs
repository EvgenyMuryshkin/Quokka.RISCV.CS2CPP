using Quokka.CS2CPP.CodeModels.CPP;

namespace Quokka.CS2CPP.CodeWriters.CPP.Declaration
{
    public class NamespaceMembersCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitIncludeCPPModel(IncludeCPPModel model)
            => Invoke<IncludeCPPModelVisitor>(model);

        public override void VisitClassCPPModel(ClassCPPModel model)
            => Invoke<ClassCPPModelVisitor>(model);
    }
}
