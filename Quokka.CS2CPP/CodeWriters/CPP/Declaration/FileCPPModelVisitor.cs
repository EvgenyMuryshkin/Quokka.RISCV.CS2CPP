using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;

namespace Quokka.CS2CPP.CodeWriters.CPP.Declaration
{
    public class FileCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitNamespaceCPPModel(NamespaceCPPModel model) 
            => Invoke<NamespaceCPPModelVisitor>(model);

        public override void VisitIncludeCPPModel(IncludeCPPModel model) 
            => Invoke<IncludeCPPModelVisitor>(model);

        public override void VisitFileCPPModel(FileCPPModel model)
        {
            var guardName = $"{Context.FileName}_H";
            AppendLine($"#ifndef {guardName}");
            AppendLine($"#define {guardName}");

            foreach (var child in model.Members)
            {
                Visit(child);
            }

            AppendLine($"#endif");
        }

        public static string Translate(CodeWriterContext context, FileCPPModel model)
        {
            var writer = new FileCPPModelVisitor()
            {
                Context = context
            };

            writer.Visit(model);
            return writer.ToString();
        }
    }
}
