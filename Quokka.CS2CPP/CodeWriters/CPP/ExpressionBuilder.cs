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

        public override void VisitIdentifierExpressionCPPModel(IdentifierExpressionCPPModel model)
        {
            Expression = model.Identifier;
        }

        static Dictionary<ExpressionTypeCPPModel, string> _lookup = new Dictionary<ExpressionTypeCPPModel, string>()
        {
            { ExpressionTypeCPPModel.Equal,          "=="    },
            { ExpressionTypeCPPModel.NotEqual,       "!="    },
            { ExpressionTypeCPPModel.Less,           "<"     },
            { ExpressionTypeCPPModel.LessOrEqual,    "<="    },
            { ExpressionTypeCPPModel.Greater,        ">"     },
            { ExpressionTypeCPPModel.GreaterOrEqual, ">="    },
            { ExpressionTypeCPPModel.Add, "+"    },
            { ExpressionTypeCPPModel.Sub, "-"    },
            { ExpressionTypeCPPModel.Mult, "*"    },
            { ExpressionTypeCPPModel.Div, "/"    },
        };

        string ToExpressionType(ExpressionTypeCPPModel op)
        {
            if (!_lookup.ContainsKey(op))
                throw new Exception($"Unsupported operation type: {op}");

            return _lookup[op];

        }
        public override void VisitBinaryExpressionCPPModel(BinaryExpressionCPPModel model)
        {
            var left = Invoke<ExpressionBuilder>(model.Left).Expression;
            var right = Invoke<ExpressionBuilder>(model.Right).Expression;
            Expression = $"({left} {ToExpressionType(model.Type)} {right})";
        }
    }
}
