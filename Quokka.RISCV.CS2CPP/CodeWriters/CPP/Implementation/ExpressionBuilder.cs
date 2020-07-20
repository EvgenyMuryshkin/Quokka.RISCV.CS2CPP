using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.CS2CPP.CodeWriters.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quokka.RISCV.CS2CPP.CodeWriters.CPP.Implementation
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

        static Dictionary<BinaryExpressionTypeCPPModel, string> _binaryExpressionLookup = new Dictionary<BinaryExpressionTypeCPPModel, string>()
        {
            { BinaryExpressionTypeCPPModel.Equal, "==" },
            { BinaryExpressionTypeCPPModel.NotEqual, "!=" },
            { BinaryExpressionTypeCPPModel.Less, "<"     },
            { BinaryExpressionTypeCPPModel.LessOrEqual, "<=" },
            { BinaryExpressionTypeCPPModel.Greater, ">"     },
            { BinaryExpressionTypeCPPModel.GreaterOrEqual, ">=" },
            { BinaryExpressionTypeCPPModel.Add, "+" },
            { BinaryExpressionTypeCPPModel.Sub, "-" },
            { BinaryExpressionTypeCPPModel.Mult, "*" },
            { BinaryExpressionTypeCPPModel.Div, "/" },
            { BinaryExpressionTypeCPPModel.RightShift, ">>" },
            { BinaryExpressionTypeCPPModel.LeftShift, "<<" },
        };

        string ToBinaryExpressionType(BinaryExpressionTypeCPPModel op)
        {
            if (!_binaryExpressionLookup.ContainsKey(op))
                throw new Exception($"Unsupported binary operation type: {op}");

            return _binaryExpressionLookup[op];

        }

        static Dictionary<UnaryExpressionTypeCPPModel, string> _unaryOperationLookup = new Dictionary<UnaryExpressionTypeCPPModel, string>()
        {
            { UnaryExpressionTypeCPPModel.Inclement, "++" },
            { UnaryExpressionTypeCPPModel.Decrement, "--" },
        };

        string ToUnaryExpressionType(UnaryExpressionTypeCPPModel op)
        {
            if (!_unaryOperationLookup.ContainsKey(op))
                throw new Exception($"Unsupported unary operation type: {op}");

            return _unaryOperationLookup[op];
        }

        public override void VisitBinaryExpressionCPPModel(BinaryExpressionCPPModel model)
        {
            var left = Invoke<ExpressionBuilder>(model.Left).Expression;
            var right = Invoke<ExpressionBuilder>(model.Right).Expression;
            Expression = $"({left} {ToBinaryExpressionType(model.Type)} {right})";
        }

        public override void VisitPrefixUnaryExpressionCPPModel(PrefixUnaryExpressionCPPModel model)
        {
            var operand = Invoke<ExpressionBuilder>(model.Operand).Expression;
            Expression = $"({ToUnaryExpressionType(model.Type)}{operand})";
        }

        public override void VisitPostfixUnaryExpressionCPPModel(PostfixUnaryExpressionCPPModel model)
        {
            var operand = Invoke<ExpressionBuilder>(model.Operand).Expression;
            Expression = $"({operand}{ToUnaryExpressionType(model.Type)})";
        }

        public override void VisitArgumentCPPModel(ArgumentCPPModel model)
        {
            Expression = Invoke<ExpressionBuilder>(model.Expression).Expression;
        }

        public override void VisitLocalInvocationCPPModel(LocalInvocationCPPModel model)
        {
            var args = model.Arguments.Select(arg => Invoke<ExpressionBuilder>(arg).Expression);
            var argsList = string.Join(", ", args);
            Expression = $"{model.Method}({argsList})";
        }

        public override void VisitDataCPPModel(DataCPPModel model)
        {
            var initializers = model.Initializers.Select(i => Invoke<ExpressionBuilder>(i).Expression);

            Expression = $"{TypeLookup.LookupCPPTypeName(model.DataType)} {string.Join(", ", initializers)}";
        }

        public override void VisitDataInitializerCPPModel(DataInitializerCPPModel model)
        {
            if (model.Initializer != null)
            {
                Expression = $"{model.Name} = {Invoke<ExpressionBuilder>(model.Initializer).Expression}";
            }
            else
            {
                Expression = $"{model.Name}";
            }
        }

        public override void VisitElementAccessCPPModel(ElementAccessCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model.Expression).Expression;
            var args = model.Arguments.Select(arg => $"[{Invoke<ExpressionBuilder>(arg).Expression}]");

            Expression = $"{expression}{string.Join("", args)}";
        }

        public override void VisitCastCPPModel(CastCPPModel model)
        {
            var type = TypeLookup.LookupCPPTypeName(model.Type);
            var expression = Invoke<ExpressionBuilder>(model.Expression).Expression;

            Expression = $"({type}){expression}";
        }

        public override void VisitParenthesizedExpressionCPPModel(ParenthesizedExpressionCPPModel model)
        {
            var expression = Invoke<ExpressionBuilder>(model.Expression).Expression;
            Expression = $"({expression})";
        }
    }
}
