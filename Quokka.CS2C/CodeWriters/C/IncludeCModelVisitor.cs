using Quokka.CS2C.CodeModels.C;

namespace Quokka.CS2C.CodeWriters.C
{
    public class IncludeCModelVisitor : BaseCModelVisitor
    {
        public override void VisitIncludeCModel(IncludeCModel model)
        {
            switch (model.Type)
            {
                case IncludeTypeCModel.System:
                    AppendLine($"include <{model.Name}>");
                    break;
                case IncludeTypeCModel.User:
                    AppendLine($"include \"{model.Name}\"");
                    break;
            }
        }
    }
}
