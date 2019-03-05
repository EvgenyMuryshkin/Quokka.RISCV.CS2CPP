using Quokka.CS2CPP.CodeModels.CPP;

namespace Quokka.CS2CPP.CodeWriters.C
{
    public class FileCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitNamespaceCPPModel(NamespaceCPPModel model) 
            => Invoke<NamespaceCPPModelVisitor>(model);

        public override void VisitIncludeCPPModel(IncludeCPPModel model) 
            => Invoke<IncludeCPPModelVisitor>(model);

        public override void VisitFileCPPModel(FileCPPModel model)
        {
            foreach (var child in model.Members)
            {
                Visit(child);
            }
        }

        public static string Translate(FileCPPModel model)
        {
            var writer = new FileCPPModelVisitor();
            writer.Visit(model);
            return writer.ToString();
        }
    }
}
