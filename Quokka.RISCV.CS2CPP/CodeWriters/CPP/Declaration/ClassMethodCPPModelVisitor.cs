using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Declaration
{
    public class ClassMethodCModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitMethodCPPModel(MethodCPPModel model)
        {
            var pars = string.Join(", ", model.Parameters.Select(arg =>
            {
                var pass = "";
                switch(arg.Pass)
                {
                    case ArgumentPassCPPModel.Ref:
                        pass = " &";
                        break;
                    case ArgumentPassCPPModel.Pointer:
                        pass = "* ";
                        break;
                    case ArgumentPassCPPModel.Raw:
                        pass = " ";
                        break;
                }
                return $"{TypeLookup.LookupCPPTypeName(arg.ParameterType)}{pass}{arg.Name}";
            }));

            AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} {TypeLookup.LookupCPPTypeName(model.ReturnType)} {model.Name}({pars});");
        }
    }
}
