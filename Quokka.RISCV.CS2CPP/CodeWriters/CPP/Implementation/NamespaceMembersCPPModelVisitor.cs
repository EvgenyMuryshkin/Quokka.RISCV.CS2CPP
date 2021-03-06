﻿using Quokka.RISCV.CS2CPP.CodeModels.CPP;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{
    public class NamespaceMembersCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitIncludeCPPModel(IncludeCPPModel model)
            => Invoke<IncludeCPPModelVisitor>(model);

        public override void VisitClassCPPModel(ClassCPPModel model)
            => Invoke<ClassCPPModelVisitor>(model);
    }
}
