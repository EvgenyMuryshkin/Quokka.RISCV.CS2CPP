﻿using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.CS2CPP.CodeWriters.CPP
{

    public class ClassCPPModelVisitor : BaseCPPModelVisitor
    {
        public override void VisitClassCPPModel(ClassCPPModel model)
        {
            var delcarationParts = new[]
            {
                CPPModelTools.Modifiers(model.Modifiers, ModifersFlag.AccessTypeCPPModel),
                "class",
                model.Name
            };

            var declarationLine = string.Join(" ", delcarationParts.Where(p => !string.IsNullOrWhiteSpace(p)));
            AppendLine($"{declarationLine}");
            OpenBlock();
            Invoke<ClassMembersCPPModelVisitor>(model.Members);
            CloseBlockWithSemicolon();
        }
    }
}
