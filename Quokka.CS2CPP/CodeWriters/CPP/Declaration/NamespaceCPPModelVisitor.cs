using Quokka.CS2CPP.CodeModels.CPP;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.CS2CPP.CodeWriters.CPP.Declaration
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
