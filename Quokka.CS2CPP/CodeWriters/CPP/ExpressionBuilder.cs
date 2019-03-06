using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;

namespace Quokka.CS2CPP.CodeWriters.CPP
{
    public class ExpressionBuilder : BaseCPPModelVisitor
    {
        public string Expression { get; set; }

        public override void VisitLiteralExpressionCPPModel(LiteralExpressionCPPModel model)
        {
            Expression = model.Value;
        }
    }
}
