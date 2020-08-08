using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{
    public class ClassFieldCModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitFieldCPPModel(FieldCPPModel model)
        {
            if (model.FieldType.IsArray)
            {
                var size = "";

                switch (model.Initializer)
                {
                    case ArrayCreationExpressionCPPModel ac:
                        size = Invoke<ExpressionBuilder>(ac.Rank).Expression;
                        break;
                    default:
                        Unsupported(model, "Array initializer not suported");
                        break;
                }

                AppendLine($"{TypeLookup.LookupCPPTypeName(model.FieldType.GetElementType())} {Context.Class.Name}::{model.Name}[{size}] = {{0}};");
            }
            else
            {
                var initializer = Invoke<ExpressionBuilder>(model.Initializer).Expression;
                AppendLine($"{TypeLookup.LookupCPPTypeName(model.FieldType)} {Context.Class.Name}::{model.Name} = {initializer};");
            }
        }
    }
}
