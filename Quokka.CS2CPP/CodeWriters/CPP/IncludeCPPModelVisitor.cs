using Quokka.CS2CPP.CodeModels.CPP;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class IncludeCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitIncludeCPPModel(IncludeCPPModel model)
        {
            switch (model.Type)
            {
                case IncludeTypeCPPModel.System:
                    AppendLine($"include <{model.Name}>");
                    break;
                case IncludeTypeCPPModel.User:
                    AppendLine($"include \"{model.Name}\"");
                    break;
            }
        }
    }
}
