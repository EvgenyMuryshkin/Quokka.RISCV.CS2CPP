using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;

namespace Quokka.CS2CPP.CodeWriters.CPP
{

    public class ClassCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitClassCPPModel(ClassCPPModel model)
        {
            AppendLine($"{CPPModelTools.Modifiers(model.Modifiers)} class {model.Name}");
            OpenBlock();
            Invoke<ClassMembersCPPModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
