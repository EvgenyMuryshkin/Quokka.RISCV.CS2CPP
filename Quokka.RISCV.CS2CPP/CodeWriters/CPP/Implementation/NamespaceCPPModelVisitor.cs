using Quokka.RISCV.CS2CPP.CodeModels.CPP;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{

    public class NamespaceCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitNamespaceCPPModel(NamespaceCPPModel model)
        {
            Context.Namespace = model;
            AppendLine($"namespace {model.Namespace}");
            OpenBlock();
            Invoke<NamespaceMembersCPPModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
