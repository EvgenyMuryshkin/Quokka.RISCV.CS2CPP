using Quokka.CS2C.CodeModels.C;

namespace Quokka.CS2C.CodeWriters.C
{
    public class NamespaceMembersCModelVisitor : BaseCModelVisitor
    {
        public override void VisitIncludeCModel(IncludeCModel model)
            => Invoke<IncludeCModelVisitor>(model);

        public override void VisitClassCModel(ClassCModel model)
            => Invoke<ClassCModelVisitor>(model);
    }
}
