using Quokka.CS2C.CodeModels.C;

namespace Quokka.CS2C.CodeWriters.C
{
    public class FileCModelVisitor : BaseCModelVisitor
    {
        public override void VisitNamespaceCModel(NamespaceCModel model) 
            => Invoke<NamespaceCModelVisitor>(model);

        public override void VisitIncludeCModel(IncludeCModel model) 
            => Invoke<IncludeCModelVisitor>(model);

        public override void VisitFileCModel(FileCModel model)
        {
            foreach (var child in model.Members)
            {
                Visit(child);
            }
        }

        public static string Translate(FileCModel model)
        {
            var writer = new FileCModelVisitor();
            writer.Visit(model);
            return writer.ToString();
        }
    }
}
