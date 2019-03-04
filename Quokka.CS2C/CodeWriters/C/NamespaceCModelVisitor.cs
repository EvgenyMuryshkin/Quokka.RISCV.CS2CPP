using Quokka.CS2C.CodeModels.C;

namespace Quokka.CS2C.CodeWriters.C
{

    public class NamespaceCModelVisitor : BaseCModelVisitor
    {
        public override void VisitNamespaceCModel(NamespaceCModel model)
        {
            AppendLine($"namespace {model.Namespace}");
            OpenBlock();
            Invoke<NamespaceMembersCModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
