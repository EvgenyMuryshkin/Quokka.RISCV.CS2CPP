using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class ClassMethodCModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.ReturnType)} {model.Name}()");
            OpenBlock();
            VisitChildren(model.Members);
            CloseBlock();
        }

        public override void VisitDataCPPModel(DataCPPModel model)
        {
            if (model.Initializer != null)
            {
                var initializer = Invoke<ExpressionBuilder>(model.Initializer).Expression;
                AppendLine($"{TypeLookup.LookupCPPTypeName(model.DataType)} {model.Name} = {initializer};");
            }
            else
            {
                AppendLine($"{TypeLookup.LookupCPPTypeName(model.DataType)} {model.Name};");
            }
        }
    }
}
