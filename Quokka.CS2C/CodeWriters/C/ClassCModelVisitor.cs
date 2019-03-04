using Quokka.CS2C.CodeModels.C;
using Quokka.CS2C.CodeWriters.Tools;
using System;
using System.Collections.Generic;

namespace Quokka.CS2C.CodeWriters.C
{

    public class ClassCModelVisitor : BaseCModelVisitor
    {
        public override void VisitClassCModel(ClassCModel model)
        {
            AppendLine($"{CModelTools.Modifiers(model.Modifiers)} class {model.Name}");
            OpenBlock();
            Invoke<ClassMembersCModelVisitor>(model.Members);
            CloseBlock();
        }
    }
}
