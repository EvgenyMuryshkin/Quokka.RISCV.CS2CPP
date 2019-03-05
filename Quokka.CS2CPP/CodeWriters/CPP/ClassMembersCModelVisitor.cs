using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class ClassMembersCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.ReturnType)} {model.Name}()");
            OpenBlock();
            //Invoke<ClassMembersCPPModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
