using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{
    public class FileCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitNamespaceCPPModel(NamespaceCPPModel model) 
            => Invoke<NamespaceCPPModelVisitor>(model);

        public override void VisitIncludeCPPModel(IncludeCPPModel model) 
            => Invoke<IncludeCPPModelVisitor>(model);

        public override void VisitFileCPPModel(FileCPPModel model)
        {
            AppendLine($"#include \"{Context.FileName}.h\"");

            foreach (var child in model.Members)
            {
                Visit(child);
            }
        }

        public static string Translate(
            CodeWriterContext context,
            FileCPPModel model)
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
