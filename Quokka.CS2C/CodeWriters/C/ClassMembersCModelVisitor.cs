using Quokka.CS2C.CodeModels.C;
using Quokka.CS2C.CodeWriters.Tools;

namespace Quokka.CS2C.CodeWriters.C
{
    public class ClassMembersCModelVisitor : BaseCModelVisitor
    {
        public override void VisitMethodCModel(MethodCModel model)
        {
            AppendLine($"{CModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.ReturnType)} {model.Name}()");
            OpenBlock();
            //Invoke<ClassMembersCModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
