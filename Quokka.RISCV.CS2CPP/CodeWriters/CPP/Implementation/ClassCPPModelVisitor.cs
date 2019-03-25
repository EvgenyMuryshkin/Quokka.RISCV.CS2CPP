using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
{
    public class ClassCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitClassCPPModel(ClassCPPModel model)
        {
            Context.Class = model;
            Invoke<ClassMembersCPPModelVisitor>(model.Members);
        }
    }
}
