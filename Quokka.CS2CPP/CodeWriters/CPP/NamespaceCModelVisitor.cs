using Quokka.CS2CPP.CodeModels.CPP;

namespace Quokka.CS2CPP.CodeWriters.CPP
{

    public class NamespaceCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitNamespaceCPPModel(NamespaceCPPModel model)
        {
            AppendLine($"namespace {model.Namespace}");
            OpenBlock();
            Invoke<NamespaceMembersCPPModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
