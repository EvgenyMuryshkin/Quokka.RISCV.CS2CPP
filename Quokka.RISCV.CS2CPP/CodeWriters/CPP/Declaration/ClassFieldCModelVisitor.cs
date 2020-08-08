using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Declaration
{
    public class ClassFieldCModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitFieldCPPModel(FieldCPPModel model)
        {
            if (model.FieldType.IsArray)
            {
                AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.FieldType.GetElementType())} {model.Name}[];");
            }
            else
            {
                AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.FieldType)} {model.Name};");
            }
        }
    }
}
